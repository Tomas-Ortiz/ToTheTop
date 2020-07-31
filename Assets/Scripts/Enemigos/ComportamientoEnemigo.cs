using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
public class ComportamientoEnemigo : MonoBehaviour
{

    [Header("Referencias")]

    public Bala balaPrefab;

    [Header("Escaneo")]

    [Range(0f, 360f)]
    public float direccionVista;

    [Range(0f, 360f)]
    public float campoVision;

    public float alcanceAtaque;

    // objetivo a atacar
    public Transform objetivo;

    private SpriteRenderer spriteRenderer;

    private Collider2D colliderEnemigo;

    private Animator animator;

    //empiezan a disparar despues de 1.8 segs
    private float temporizadorDisparo = 1.8f;

    //cuando se voltea el sprite, se ajusta el vector de acuerdo a la orientación del sprite
    private Vector2 orientacionSprite;

    public float velocidadProyectil;

    public float TemporizadorDisparo { get => temporizadorDisparo; set => temporizadorDisparo = value; }

    private ControladorAudio controladorAudio;

    void Awake()
    {
        colliderEnemigo = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controladorAudio = GameObject.Find("AudioSpitter").GetComponent<ControladorAudio>();

        // se obtiene la orientacion del sprite
        orientacionSprite = spriteRenderer.flipX ? Vector2.left : Vector2.right;
    }

    void FixedUpdate()
    {
        actualizarTemporizadorDisparo();

        animator.SetBool("Grounded", true);
    }

    public bool buscarJugador()
    {
        // se calcula la distancia entre el jugador (objetivo) y el enemigo
        Vector3 distanciaObjetivo = objetivo.position - transform.position;

        // si el jugador se encuentra fuera del alcance de ataque del enemigo
        if (distanciaObjetivo.sqrMagnitude > alcanceAtaque * alcanceAtaque)
            return false;

        //Los cuaterniones se usan para rotaciones
        // Euler gira z,x,y grados alrededor del eje z,x,y, en ese orden

        Vector3 orientacionDirVista = Quaternion.Euler(0, 0, direccionVista) * orientacionSprite;

        // se calcula el angulo en grados entre dos vectores:
        // distancia al jugador (distanciaObjetivo) y la orientacion de la direccion de vista
        float angulo = Vector3.Angle(orientacionDirVista, distanciaObjetivo);

        // si el jugador se encuentra fuera del campo de vision
        if (angulo > campoVision * 0.5f)
            return false;

        //OBJETIVO DENTRO DEL AREA
        return true;
    }

    public void disparar()
    {
        // se crea y configura la bala

        GameObject bala = balaPrefab.getBala();

        Rigidbody2D rbBala = bala.gameObject.GetComponent<Rigidbody2D>();

        // se establece el origen de la bala en relacion a la posicion de este gameobject
        bala.transform.position = new Vector2(transform.position.x, transform.position.y + 0.75f);

        animator.SetTrigger("Shooting");
        controladorAudio.reproducirAudio("ataqueSpitter");

        // la velocidad de la bala depende de la distancia entre el objetivo y el enemigo
        // dicha distancia se normaliza para que la velocidad de la bala no sea tan alta
        // y se lo multiplica por la velocidad de la bala

        Vector2 velocidadBala = (objetivo.position - transform.position).normalized * velocidadProyectil;
        rbBala.velocity = new Vector2(velocidadBala.x, velocidadBala.y + 2f);
    }

    IEnumerator morir(Dañador dañador, Dañable dañable)
    {
        // animacion de morir, se espera 0.5 seg y se desaparece el objeto
        animator.SetTrigger("Death");

        controladorAudio.reproducirAudio("muerteSpitter");

        yield return new WaitForSeconds(0.5f);

        MenuFinalNivel.enemigosEliminados++;

        activarEnemigo(false);
    }

    public void recibirDaño(Dañador dañador, Dañable dañable)
    {
        animator.SetTrigger("Hit");

        //se resta la vida segun el daño del dañador
        dañable.VidaActual -= dañador.daño;

        if (dañable.VidaActual <= 0)
        {
            StartCoroutine(morir(dañador, dañable));
        }
    }

    public void orientarHaciaObjetivo()
    {
        Vector3 alObjetivo = objetivo.position - transform.position;

        // Dot calcula el producto de dos vectores
        // devuelve valor negativo si estos vectores apuntan en direcciones opuestas
        // si se encuentran en direcciones opuestas el enemigo cambia de direccion

        if (Vector3.Dot(alObjetivo, orientacionSprite) < 0)
        {
            establecerOrientacionSprite(Mathf.RoundToInt(-orientacionSprite.x));
        }
    }

    public void establecerOrientacionSprite(int orientacion)
    {
        // si está mirando a la derecha se cambia a izq
        if (orientacion == -1)
        {
            spriteRenderer.flipX = true;
            orientacionSprite = Vector2.left;
        }
        // si está mirando a la izq se cambia a derecha
        else if (orientacion == 1)
        {
            spriteRenderer.flipX = false;
            orientacionSprite = Vector2.right;
        }
    }

    public bool verificarTemporizadorDisparo()
    {
        //si todavia no se puede disparar
        if (TemporizadorDisparo > 0.0f)
        {
            return false;
        }

        // animacion de preparacion para disparar
        animator.SetTrigger("Spotted");

        return true;
    }

    private void actualizarTemporizadorDisparo()
    {
        // si es mayor a cero se empieza a decrementar en cada frame
        if (TemporizadorDisparo > 0.0f)
        {
            TemporizadorDisparo -= Time.deltaTime;
        }
    }

    private void activarEnemigo(bool activar)
    {
        Destroy(gameObject);
    }

    // Se llama a OnDrawGizmosSelected solo si se selecciona el objeto al que se adjunta el script 
    // se dibuja en la vista de la escena
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //Dibujar el cono de vista

        // se obtiene la orientacion del sprite
        Vector3 orientacionCono = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // en base a la orientacion del sprite, se establece la orientación del cono de vista
        // solo se modifica en z, segun los grados de la direccion de la vista
        orientacionCono = Quaternion.Euler(0, 0, spriteRenderer.flipX ? -direccionVista : direccionVista) * orientacionCono;

        Vector3 puntoFinalCono = transform.position + (Quaternion.Euler(0, 0, campoVision * 0.5f) * orientacionCono);

        //color del cono (rgba)
        Handles.color = new Color(0, 1, 0, 0.2f);

        // se dibuja el circulo
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (puntoFinalCono - transform.position), campoVision, alcanceAtaque);
    }

#endif
}

using UnityEngine;

public class Dañador : MonoBehaviour
{

    // daño que realiza el dañador
    public int daño = 1;

    // capas objetivos del dañador
    public LayerMask capaAtacable;

    public bool golpearTriggers;

    // filtrar resultados del contacto con otros colliders 2D
    private ContactFilter2D filtroContactoAtaque;

    // vectores que definen un rectangulo para crear el hitbox
    private Vector2 puntoA, puntoB;

    [Header("Hitbox")]
    // desplazamiento de la hitbox
    public Vector2 desplazamientoHitbox = Vector2.zero;

    // tamaño o area del rectángulo (hitbox)
    public Vector2 tamañoHitbox = Vector2.zero;

    // arreglo de colliders de tamaño equivalente a la cantidad de resultados maximos que se desean obtener
    private Collider2D[] resultsAtaqueCollider = new Collider2D[2];

    private Collider2D colliderAtacable;

    // awake es llamado una vez al principio, antes que start, cuando se carga la instancia de este script
    void Awake()
    {
        configurarContactFilter();
    }

    void FixedUpdate()
    {
        // se crea el area rectangular (hitbox) del dañador
        crearHitbox();

        //Debug.DrawLine(puntoA, puntoB, Color.red);

        // OverlapArea comprueba si un colisionador cae dentro del hitbox del atacante
        // los colisionadores se van a filtrar por las capas atacables especificadas
        // devuelve un int que indica la cantidad de colliders que entraron al hitbox
        int cantCollidersAtacados = Physics2D.OverlapArea(puntoA, puntoB, filtroContactoAtaque, resultsAtaqueCollider);

        // se recorren todos los colliders que entro al hitbox del dañador
        for (int i = 0; i < cantCollidersAtacados; i++)
        {
            colliderAtacable = resultsAtaqueCollider[i];

            // se obtiene el script dañable del collider que entró al hitbox
            Dañable dañable = colliderAtacable.GetComponent<Dañable>();

            // si el collider tiene el script de dañable
            if (dañable)
            {
                // la colision que entró al hitbox recibe daño, y se pasa como parametro este dañador
                // para indicar cuanto daño debe recibir el dañable
                dañable.recibirDaño(this);

                // si el dañador es una bala y colisiona con un dañable el gameobject de este script (bala) se destruye
                if (this.tag == "Bala")
                {
                    Bala.destruirBala(this.gameObject);
                }
            }
        }
    }

    void crearHitbox()
    {
        // se obtiene la escala del dañador
        Vector2 escalaDañador = transform.lossyScale;

        // scale multiplica dos vectores y devuelve el vector multiplicado
        // util para que el tamaño y desplazamiento del hitbox sea acorde a la escala del dañador

        Vector2 desplazamientoHitboxEscalado = Vector2.Scale(desplazamientoHitbox, escalaDañador);

        // Debug.Log("Direccion de ataque: " + direccionAtaque);

        Vector2 tamañoHitboxEscalado = Vector2.Scale(tamañoHitbox, escalaDañador);

        // punto a y b son los vertices del rectángulo

        // tamañoHitboxEscalado es el vector diagonal del rectángulo, 
        // y se lo multiplica por 0.5 para obtener la mitad del rectangulo

        // puntoA depende de la posicionn del dañador, el desplazamiento y tamaño del hitbox escalado
        // puntoB depende del puntoA y del hitbox escalado

        puntoA = (Vector2)transform.position + desplazamientoHitboxEscalado - tamañoHitboxEscalado * 0.5f;
        puntoB = puntoA + tamañoHitboxEscalado;
    }

    private void configurarContactFilter()
    {
        // se filtran los resultados de contacto de la/s capa/s especificada/s
        filtroContactoAtaque.layerMask = capaAtacable;

        // habilitar el filtrado de contacto por capa
        filtroContactoAtaque.useLayerMask = true;

        // se habilita o no el contacto con colisiones trigger
        filtroContactoAtaque.useTriggers = golpearTriggers;
    }
}

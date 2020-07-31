using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{
    private Rigidbody2D rbPersonaje;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private Dañable dañable;

    public Bala balaPrefab;

    public float velocidadBala;

    public float tiempoAntesDisparar;

    private float temporizadorDisparo;

    private bool puedeDisparar;

    [HideInInspector]
    public bool alcanzoFinalNivel;

    private float movimientoVertical;

    private float reduccionSalto;

    public float alturaSalto;

    public float velocidadMov;

    private bool grounded;

    private bool bloquearMovimiento = false;

    public bool Grounded { get => grounded; set => grounded = value; }
    public float MovimientoVertical { get => movimientoVertical; set => movimientoVertical = value; }

    private float posBala_x;

    private float posBala_y;

    private ControladorAudio controladorAudio;

    // al principio, se obtienen algunos componentes
    // y se setean valores predeterminados
    void Start()
    {
        rbPersonaje = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        dañable = GetComponent<Dañable>();

        controladorAudio = GameObject.Find("ControladorAudio").GetComponent<ControladorAudio>();

        alcanzoFinalNivel = false;

        temporizadorDisparo = tiempoAntesDisparar;

        movimientoVertical = 22f;

        reduccionSalto = 50f;
    }

    // en fixedUpdate no hace falta multiplicar por deltaTime
    void FixedUpdate()
    {
        //si no está bloqueado el movimiento se puede mover y saltar
        if (!bloquearMovimiento)
        {
            // movimiento personaje
            mover();

            // se comprueba la animacion de saltar
            verificarAnimacionSalto();

            // si toca piso salta automaticamente
            if (grounded)
            {
                controladorAudio.reproducirAudio("saltoJugador");

                saltar();
            }
        }

        disparar();

        actualizarTemporizadorDisparo();
    }

    void verificarAnimacionSalto()
    {
        if (grounded)
        {
            movimientoVertical = 22f;
            animator.SetFloat("VerticalSpeed", movimientoVertical);
        }

        // si el jugador saltó entonces se reproduce la animación de saltar progresivamente en cada frame
        // hasta que la animación termine

        movimientoVertical -= reduccionSalto * Time.deltaTime;
        animator.SetFloat("VerticalSpeed", movimientoVertical);

        // si se termina la animación
        if (movimientoVertical <= -25)
        {
            animator.SetFloat("VerticalSpeed", 0);
            movimientoVertical = 22f;
        }
    }

    void saltar()
    {
        // Antes de saltar, la velocidad en y se hace 0 (se reinicia)
        // ya que la velocidad de los saltos anteriores se acumula y salta muy alto

        rbPersonaje.velocity = new Vector2(rbPersonaje.velocity.x, 0f);

        Vector2 fuerza = new Vector2(0f, alturaSalto);

        rbPersonaje.AddForce(fuerza, ForceMode2D.Impulse);
    }

    void mover()
    {
        // direccion negativa izq (a), direccion positiva derecha (d)

        float direccion = Input.GetAxis("Horizontal");

        darVueltaSprite(direccion);

        animator.SetFloat("HorizontalSpeed", Mathf.Abs(rbPersonaje.velocity.x));

        // la velocidad en y no se modifica
        rbPersonaje.velocity = new Vector2(direccion * velocidadMov, rbPersonaje.velocity.y);
    }

    void disparar()
    {
        // si se aprieta el click izq y puede disparar (temporizador)
        if (Input.GetMouseButton(0) && puedeDisparar)
        {
            // se crea la bala (prefab)
            GameObject bala = balaPrefab.getBala();

            setPosOrigenBala();

            animator.SetBool("HoldingGun", true);

            controladorAudio.reproducirAudio("disparoPersonaje");

            // la posicion de la bala es en base a la posicion del personaje
            bala.transform.position = new Vector2(transform.position.x + posBala_x, transform.position.y + posBala_y);

            Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();

            orientarBala();

            // velocida de la bala en x
            rbBala.velocity = new Vector2(velocidadBala, 0f);
        }
        else
        {
            animator.SetBool("HoldingGun", false);
        }
    }

    public void muerePersonaje(Dañador dañador, Dañable dañable)
    {
        // si el personaje se queda sin vidas se termina el nivel
        terminarNivel();
    }

    public void personajeVisible(bool visible)
    {
        // Un renderizador es lo que hace que un objeto aparezca en la pantalla
        // con enabled se hace que el objeto sea invisible o no

        Renderer personajeRenderer = this.gameObject.GetComponent<Renderer>();

        personajeRenderer.enabled = visible;
    }

    public void activarPersonaje(bool activar)
    {
        gameObject.SetActive(activar);
    }

    public void setEstadisticasPersonaje()
    {
        // se obtienen las estadisticas de la partida

        string tiempo = TiempoUI.minutos + ":" + TiempoUI.segundos;

        MenuFinalNivel.tiempo = tiempo;

        MenuFinalNivel.altura = AlturaUI.posY.ToString();

        if (dañable.VidaActual < 0)
        {
            dañable.VidaActual = 0;
        }

        MenuFinalNivel.vida = dañable.VidaActual;

        MenuFinalNivel.plataformasPisadas = ComprobacionSuelo.plataformasPisadas;
    }

    // solo se puede disparar cada cierto tiempo
    private void actualizarTemporizadorDisparo()
    {
        if (temporizadorDisparo > 0.0f)
        {
            puedeDisparar = false;
            temporizadorDisparo -= Time.deltaTime;
        }
        else
        {
            puedeDisparar = true;
            temporizadorDisparo = tiempoAntesDisparar;
        }
    }

    private void orientarBala()
    {
        // si el personaje está mirando a la izq y la bala está orientada a la derecha (valor positivo)

        if (spriteRenderer.flipX && Mathf.Sign(velocidadBala) == 1)
        {
            velocidadBala = -velocidadBala;
        }

        if (!spriteRenderer.flipX)
        {
            velocidadBala = Mathf.Abs(velocidadBala);
        }
    }

    private void setPosOrigenBala()
    {
        if (spriteRenderer.flipX)
        {
            posBala_x = -1.119f;
        }
        else
        {
            posBala_x = 1.119f;
        }
        posBala_y = 1.839f;
    }

    private void darVueltaSprite(float direccion)
    {
        if (direccion < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direccion > 0.1f)
        {
            spriteRenderer.flipX = false;
        }

        // si se está moviendo y toca piso
        if (direccion != 0 && grounded)
        {
            controladorAudio.reproducirAudio("pisadasJugador");
        }
    }

    private void terminarNivel()
    {
        // lo que se hace cuando el personaje muere

        controladorAudio.reproducirAudio("muerte");

        setEstadisticasPersonaje();

        activarPersonaje(false);

        MenuFinalNivel.tituloMenu = "HAS MUERTO!";

        configurarMenuFinalNivel();
    }

    public void configurarMenuFinalNivel()
    {
        TiempoUI.detenerTiempo = true;

        MenuFinalNivel.activarPanelFinalNivel();

        MenuFinalNivel.enemigosEliminados = 0;
    }
}


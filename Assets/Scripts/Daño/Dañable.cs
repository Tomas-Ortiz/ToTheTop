using System;
using UnityEngine;
using UnityEngine.Events;

public class Dañable : MonoBehaviour
{

    public int vidaInicial = 3;

    public bool invulnerableDespuesDeDaño = true;

    public float duracionInvulnerabilidad;

    private bool invulnerable;

    private float timerInvulnerabilidad;
    private int vidaActual;
    public int VidaActual { get => vidaActual; set => vidaActual = value; }

    // Una clase se serializa para transformarla en un formato que Unity puede almacenar y reconstruir después (bits)
    // Estas clases son visibles en el inspector, por lo que puede editar sus valores
    // y Unity lo serializará (guardará) para la próxima vez que abra el proyecto
    // las clases personalizadas que hereden de UnityEvent se tienen que hacer serializables

    [Serializable]
    public class EventoVida : UnityEvent<Dañable> { }

    [Serializable]
    public class EventoDaño : UnityEvent<Dañador, Dañable> { }

    public EventoVida setVida;

    public EventoDaño recibeDaño;

    private ControladorAudio controladorAudio;

    void Awake()
    {
        controladorAudio = GameObject.Find("ControladorAudio").GetComponent<ControladorAudio>();
    }

    void Start()
    {
        // al principio la vida actual es la vida inicial
        vidaActual = vidaInicial;

        // se invoca el evento y se requiere enviar como argumento un objeto tipo Dañable (Ellen)
        setVida.Invoke(this);

        desactivarInvulnerabilidad();
    }

    void Update()
    {
        // si esta en un periodo de invulnerabilidad
        if (invulnerable)
        {
            // se actualiza el temporizador de invulnerabilidad
            timerInvulnerabilidad -= Time.deltaTime;

            // si ya paso el tiempo de invulnerabilidad se desactiva
            if (timerInvulnerabilidad <= 0f)
            {
                desactivarInvulnerabilidad();
            }
        }
    }

    // dañador es el enemigo que hace daño a un dañable
    public void recibirDaño(Dañador dañador)
    {
        // si está en un periodo de invulnerabilidad no se hace nada
        if (invulnerableDespuesDeDaño && invulnerable)
        {
            return;
        }

        controladorAudio.reproducirAudio("dañoSpike");
        controladorAudio.reproducirAudio("dañoGeneral");

        // se resta la vida actual según el daño recibido por el dañador
        vidaActual -= dañador.daño;

        // se invoca el evento para que cambie la vida en el UI 
        setVida.Invoke(this);

        // si el personaje no esta muerto y está activada la invulnerabilidad
        if (vidaActual > 0 && invulnerableDespuesDeDaño)
        {
            activarInvulnerabilidad();
        }

        // se invoca el evento para verificar la vida actual
        // si la barra de vida llego a cero se termina el juego y se muestra un panel (ControladorPersonaje)
        if (vidaActual <= 0)
        {
            recibeDaño.Invoke(dañador, this);
        }
    }

    // recibir daño por daño de caida
    // no hay invulnerabilidad en el daño por caida
    public void recibirDañoCaida(int daño)
    {
        controladorAudio.reproducirAudio("dañoGeneral");

        vidaActual -= daño;

        setVida.Invoke(this);

        Dañador dañador = null;

        // se invoca el evento para verificar la vida actual
        // si la barra de vida llego a cero se termina el juego y se muestra un panel (ControladorPersonaje)
        if (vidaActual <= 0)
        {
            recibeDaño.Invoke(dañador, this);
        }
    }

    public void activarInvulnerabilidad()
    {
        invulnerable = true;
        timerInvulnerabilidad = duracionInvulnerabilidad;
    }

    public void desactivarInvulnerabilidad()
    {
        invulnerable = false;
    }
}

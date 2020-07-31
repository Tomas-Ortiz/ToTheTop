using UnityEngine;

public class Spitter : MonoBehaviour
{
    public float temporizadorDisparo;
    private ComportamientoEnemigo comportamientoEnemigo;

    void Start()
    {
        comportamientoEnemigo = GetComponent<ComportamientoEnemigo>();
    }

    void FixedUpdate()
    {
        comportamientoEnemigo.orientarHaciaObjetivo();

        bool jugadorEncontrado = comportamientoEnemigo.buscarJugador();

        // si el jugador se encuentra dentro del cono de vision
        if (jugadorEncontrado)
        {
            // se verifica si se puede disparar
            bool sePuedeDisparar = comportamientoEnemigo.verificarTemporizadorDisparo();

            if (sePuedeDisparar)
            {
                comportamientoEnemigo.disparar();

                // se espera 1 seg para volver a disparar
                comportamientoEnemigo.TemporizadorDisparo = temporizadorDisparo;
            }
        }
    }
}

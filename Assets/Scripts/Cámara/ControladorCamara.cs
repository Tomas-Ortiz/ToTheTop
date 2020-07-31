using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    public Transform objetivoPersonaje;

    public float velocidadCamara;

    public Vector3 posCamara;

    // una cámara de seguimiento debe implementarse en LateUpdate, y se llama despues del update
    // porque rastrea objetos que podrían haberse movido dentro de update
    void LateUpdate()
    {
        // A la posicion del jugador se le suma la posicion deseada de la camara (vector fijo)
        // la posicion de la camara es en base a la posicion actual del jugador
        Vector3 posDestinoCamara = objetivoPersonaje.position + posCamara;

        // Lerp recibe 2 vectores (punto de inicio y destino) y un interpolante
        // se pasa la posicion actual de la camara (inicio) y la nueva posicion destino de la camara (destino)
        // De esta manera, se mueve gradualmente la camara entre dichos puntos 

        // el interpolante interpola entre dos puntos
        // por ej. si es igual 0.5 devuelve el punto medio entre el punto inicial y destino
        float interpolante = velocidadCamara * Time.deltaTime;

        Vector3 posFinalCamara = Vector3.Lerp(transform.position, posDestinoCamara, interpolante);

        // se asigna la posicion final de la camara
        transform.position = posFinalCamara;

        //efecto visual en el que se realiza una rotacion en la camara en x,y segun la posicion del personaje
        transform.LookAt(objetivoPersonaje);

        Debug.DrawLine(Vector3.zero, posFinalCamara, Color.yellow);
    }

}

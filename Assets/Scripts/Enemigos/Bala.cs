using UnityEngine;

public class Bala : MonoBehaviour
{
    public float tiempoAntesDestruirse;

    private void FixedUpdate()
    {
        actualizarTiempoDestruccion();
    }

    public GameObject getBala()
    {
        //se retorna una copia o clone de este gameobject (prefab)
        return Instantiate(gameObject);
    }

    private void actualizarTiempoDestruccion()
    {
        //cuando el temporizador llega a cero se destruye la bala
        if (tiempoAntesDestruirse > 0.0f)
        {
            tiempoAntesDestruirse -= Time.deltaTime;
        }
        else
        {
            destruirBala(this.gameObject);
        }
    }

    public static void destruirBala(GameObject bala)
    {
        Destroy(bala);
    }

}

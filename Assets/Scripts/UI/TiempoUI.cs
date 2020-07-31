using UnityEngine;
using UnityEngine.UI;

public class TiempoUI : MonoBehaviour
{
    private Text tiempoUI;
    private float tiempoTranscurrido;

    public static bool detenerTiempo;

    public static string minutos, segundos;
    void Start()
    {
        tiempoUI = this.GetComponent<Text>();
        detenerTiempo = false;
        tiempoUI.text = "00:00";
    }

    void Update()
    {
        if (!detenerTiempo)
        {
            // se va incrementando el tiempo transcurrido
            tiempoTranscurrido += Time.deltaTime;

            // se obtienen los minutos y segundos en cada frame
            calcularTIempo(tiempoTranscurrido);

            tiempoUI.text = minutos + ":" + segundos;
        }
    }

    private void calcularTIempo(float tiempoTranscurrido)
    {
        // mathf.floor para obtener un numero entero y no en decimales
        // ToString para dar un formato a los minutos y segundos 

        //si tiempoPasado = 125 (seg) entonces minutos = 2 y segundos = 5 (02:05)

        minutos = Mathf.Floor(tiempoTranscurrido / 60).ToString("00");

        segundos = Mathf.Floor(tiempoTranscurrido % 60).ToString("00");

    }

}

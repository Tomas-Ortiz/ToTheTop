using UnityEngine;
using UnityEngine.UI;

public class AlturaUI : MonoBehaviour
{
    public GameObject personaje;

    private Text altura;

    public static int posY;

    void Start()
    {
        altura = this.GetComponent<Text>();
        posY = (int)personaje.transform.position.y;
        altura.text = posY.ToString() + " MTS.";
    }

    void Update()
    {
        // se obtiene la altura en todo momento y se muestra en pantalla

        posY = (int)personaje.transform.position.y;

        if (posY < 0)
        {
            posY = 0;
        }
        altura.text = posY.ToString() + " MTS.";
    }

}

using UnityEngine;
public class VidaUI : MonoBehaviour
{
    //Script Dañable asignada al personaje
    public Dañable personajeDañable;

    // prefab del icono de vida (objeto original)
    public GameObject iconoVidaPrefab;

    //Distancia entre iconos de vida
    private float distanciaEntreIconos = 0.038f;

    // animator de cada icono de vida
    private Animator[] iconoVidaAnimadores;

    void Start()
    {
        // se crean un array de tantos animadores como vidas tenga el personaje
        // cada icono tiene su propio animator
        iconoVidaAnimadores = new Animator[personajeDañable.vidaInicial];

        // se recorre y configura cada icono de vida del UI
        for (int i = 0; i < personajeDañable.vidaInicial; i++)
        {
            // se hace una copia del prefab del icono de vida (clones del iconoVidaPrefab)
            // y se crean tantos iconos de vida como vida tenga el personaje
            GameObject iconoVida = Instantiate(iconoVidaPrefab);

            // a cada nuevo icono creado se le establece como padre el transform del VidaCanvas
            // si se mueve el padre se mueve el hijo
            iconoVida.transform.SetParent(transform);

            // se obtiene el RectTransform para cada icono de vida
            RectTransform iconoVidaRect = iconoVida.GetComponent<RectTransform>();

            // la posicion del RectTransform en relacion a los puntos de anclaje
            // se lo coloca en left = 615 y right = -615, top = 4 y bottom = -4
            iconoVidaRect.anchoredPosition = new Vector2(615, -4);

            // tamaño (ancho, alto) del RectTransform en relacion a las distancias entre los puntos de anclaje
            // zero equivale a ancho = 0 y alto = 0
            iconoVidaRect.sizeDelta = Vector2.zero;

            // se ajustan las posiciones de los puntos de anclaje min y max (solo en x)
            // al anclaje min y max de cada icono se le suma el anclaje min y max anterior respectivamente,
            // para que cada icono se encuentre separado uno de otro

            iconoVidaRect.anchorMin += new Vector2(distanciaEntreIconos, 0) * i;
            iconoVidaRect.anchorMax += new Vector2(distanciaEntreIconos, 0) * i;

            // se obtiene el componente animator de cada gameObject iconoVida (HealthIcon)
            // y se lo agrega al array de animadores

            iconoVidaAnimadores[i] = iconoVida.GetComponent<Animator>();
        }
    }

    // recibe como parametro un objeto de tipo Dañable para obtener la vida actual
    // y establecer los corazones activos e inactivos
    public void activarCorazonesUI(Dañable dañable)
    {
        // se recorren todos los animators de cada icono de vida
        for (int i = 0; i < iconoVidaAnimadores.Length; i++)
        {
            // para cada animator, se establece el parametro active como true o false
            // true = corazon activo (en rojo), false = corazon inactivo (vacio)
            // segun la vida actual del personaje dañable 
            iconoVidaAnimadores[i].SetBool("Active", personajeDañable.VidaActual >= i + 1);
        }

    }


}

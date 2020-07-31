using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuFinalNivel : MonoBehaviour
{
    public static string altura, tiempo, tituloMenu;
    public static int vida, plataformasPisadas, enemigosEliminados;

    // para volver a jugar se vuelve a cargar la escena del nivel 1 (buildIndex = 1)
    public void volverJugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void mostrarEstadisticasUI()
    {
        Text textoEstadisticas = GameObject.Find("contenidoEstadisticas").GetComponent<Text>();

        textoEstadisticas.text = "Vidas restantes: " + vida +
        "\nTiempo alcanzado: " + tiempo +
        "\nAltura llegada: " + altura + " MTS." +
        "\nPlataformas utilizadas: " + plataformasPisadas +
        "\nEnemigos eliminados: " + enemigosEliminados;
    }

    public void volverMenuPrincipal()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void setTituloUI()
    {
        // Se establece un titulo u otro segun si murio o paso el nivel
        TMP_Text tituloFinalNivel = GameObject.Find("PanelFinalNivel/TituloFinalNivel").GetComponent<TMP_Text>();

        tituloFinalNivel.text = tituloMenu;

        modificarPosicionTitulo(tituloFinalNivel);
    }

    public static void activarPanelFinalNivel()
    {
        // se obtiene el gameobject padre para obtener gameobject desactivado
        GameObject UI = GameObject.Find("----- UI -----");
        GameObject nivelFinalCanvas = UI.transform.Find("finalNivelCanvas").gameObject;

        // se muestra el panel en pantalla
        nivelFinalCanvas.SetActive(true);

        setTituloUI();

        // se muestran las estadisticas del jugador en el panel
        mostrarEstadisticasUI();
    }

    private static void modificarPosicionTitulo(TMP_Text titulo)
    {
        RectTransform tituloTransform = titulo.GetComponent<RectTransform>();

        // se modifica la posicion del titulo segun el titulo
        if (tituloMenu.Equals("HAS MUERTO!"))
        {
            tituloTransform.localPosition = new Vector2(37f, tituloTransform.localPosition.y);
        }
    }
}

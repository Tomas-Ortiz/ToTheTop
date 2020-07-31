using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // se carga la escena del nivel 1 (buildIndex = 1)
    // buildIndex indica el indice de la escena actual
    // los indices de las escenas se configuran en build settings
    public void jugarPrimerNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void salirJuego()
    {
        Application.Quit();
    }

}

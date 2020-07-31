using System.Collections;
using UnityEngine;

public class ComprobacionSuelo : MonoBehaviour
{
    private ControladorPersonaje personaje;
    private Animator animator;

    public static int plataformasPisadas;

    void Start()
    {
        personaje = GetComponent<ControladorPersonaje>();
        animator = GetComponent<Animator>();
        plataformasPisadas = 0;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // si colisiona con una colision que no se tiene que ignorar (ej. plataforma):
        if (other.collider.tag != "ColisionIgnorada")
        {
            // si cae en una plataforma valida 

            personajeEnPiso();

            reiniciarAnimacionSalto();

            // si se colisiona con una plataforma aérea válida ésta desaparece
            // 31 = platform
            if (other.gameObject.layer == 31 && other.collider.tag != "Base" && other.collider.tag != "finNivel")
            {
                StartCoroutine(desaparecerPlataforma(other));
                plataformasPisadas++;
            }

            // si el jugador llega al final del nivel
            if (other.collider.tag == "finNivel")
            {
                personaje.activarPersonaje(false);

                personaje.setEstadisticasPersonaje();

                MenuFinalNivel.tituloMenu = "PASASTE DE NIVEL!";

                personaje.configurarMenuFinalNivel();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag != "ColisionIgnorada")
        {
            personaje.Grounded = false;
            animator.SetBool("Grounded", personaje.Grounded);
        }
    }

    // corrutina para usar el yield WaitForSeconds
    IEnumerator desaparecerPlataforma(Collision2D collision)
    {
        // se espera 0,1 seg para desaparecer la plataforma
        yield return new WaitForSeconds(0.1f);

        collision.gameObject.SetActive(false);
    }

    private void personajeEnPiso()
    {
        personaje.Grounded = true;
        animator.SetBool("Grounded", personaje.Grounded);
    }

    private void reiniciarAnimacionSalto()
    {
        personaje.MovimientoVertical = 0f;
        animator.SetFloat("VerticalSpeed", personaje.MovimientoVertical);
    }
}

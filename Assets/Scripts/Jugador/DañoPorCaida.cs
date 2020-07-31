using UnityEngine;

public class DañoPorCaida : MonoBehaviour
{
    private Animator animator;

    private Rigidbody2D rb2d;

    // se obtiene el dañable del personaje
    public Dañable dañable;
    private int caidaRecorrida;

    private int alturaMaxima, cayoEn;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb2d = this.GetComponent<Rigidbody2D>();
        caidaRecorrida = 0;
        alturaMaxima = 0;
        cayoEn = 0;
    }

    void Update()
    {
        bool grounded = animator.GetBool("Grounded");

        int pos_y = (int)this.transform.position.y;

        // se calculan los metros de caida a partir de la pos y del personaje 
        calcularMtsCaida(grounded, pos_y);

        // segun los mts caidos va a variar el daño
        if (caidaRecorrida >= 14)
        {
            calcularDaño(caidaRecorrida);
        }
    }

    private void calcularMtsCaida(bool grounded, int pos_y)
    {
        // si el personaje no está en el piso y el salto está en plena subida
        // se obtiene la altura maxima alcanzada (pos y actual del personaje)
        if (!grounded && rb2d.velocity.y > 0)
        {
            alturaMaxima = pos_y;
        }

        // cuando cae al piso se calculan los metros de caída
        if (grounded)
        {
            //se obtiene la pos y actual del personaje cuando toca piso
            cayoEn = pos_y;

            // se calcula la caida recorrida (mts)
            caidaRecorrida = alturaMaxima - cayoEn;

            // una vez toca el piso, la altura maxima equivale a su altura actual
            alturaMaxima = cayoEn;
        }
    }

    private void calcularDaño(int caidaRecorrida)
    {
        int daño = 0;

        if (caidaRecorrida >= 14 && caidaRecorrida < 17)
        {
            daño = 1;
        }
        else if (caidaRecorrida >= 17 && caidaRecorrida < 20)
        {
            daño = 2;
        }
        else if (caidaRecorrida >= 20 && caidaRecorrida < 23)
        {
            daño = 3;
        }
        else if (caidaRecorrida >= 23)
        {
            daño = dañable.VidaActual;
        }

        enviarDañoCaida(daño);
    }

    // se envia el daño por caida al dañable (personaje)
    private void enviarDañoCaida(int daño)
    {
        dañable.recibirDañoCaida(daño);
    }
}

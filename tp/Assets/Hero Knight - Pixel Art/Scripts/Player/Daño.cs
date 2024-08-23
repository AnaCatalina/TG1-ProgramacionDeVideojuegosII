using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daño : MonoBehaviour
{
    public float vida;
    public BarraDeVida barra;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        barra.InicializarBarraVida(vida);
    }

    public void TomarDano(float danio)
    {
        anim.SetTrigger("Hurt");
        vida -= danio;
        barra.CambiarVidaActual(vida);
        if(vida <= 0f)
        {
            anim.SetTrigger("Death");
            AudioManager.instance.Reproducir(11);
            gameObject.SetActive(false);
        }
    }

    public float InformeVida()
    {
        return vida;
    }
}

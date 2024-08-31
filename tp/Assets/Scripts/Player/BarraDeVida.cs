using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    private void Awake(){
        slider = GetComponent<Slider>();
    }

    public float VidaActual()
    {
        return slider.value;
    }

    public void CambiarVidaMaxima(float vidaMax) => slider.maxValue = vidaMax;

    public void CambiarVidaActual(float cantVida){
        slider.value = cantVida;
    }

    public void InicializarBarraVida(float cantVida){
        CambiarVidaMaxima(cantVida);
        CambiarVidaActual(cantVida);
    }
}

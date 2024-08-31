using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject btnPausa;
    public GameObject menuPausa;
    public void Pausa()
    {
        Time.timeScale = 0f;
        btnPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        btnPausa.SetActive(true);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Cerrar()
    {
        print("Saliendo...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}

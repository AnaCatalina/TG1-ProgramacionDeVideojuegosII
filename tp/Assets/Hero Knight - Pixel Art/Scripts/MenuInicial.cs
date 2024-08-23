using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("JuegoPrincipal");
    }

    public void Salir()
    {
        print("Saliendo..");
        Application.Quit();
    }
}

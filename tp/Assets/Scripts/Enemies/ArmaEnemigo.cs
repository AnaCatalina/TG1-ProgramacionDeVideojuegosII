using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaEnemigo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Escudo"))
        {
            AudioManager.instance.Reproducir(2);
            print("Bloqueo");
        }
    }
}

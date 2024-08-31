using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrulla : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;
    public float distanciaVision = 5f;
    private Transform objetivo;
    private bool yendoAPuntoB = true;

    void Start()
    {
        objetivo = GameObject.FindGameObjectWithTag("Player").transform;
        puntoA.parent = null;
        puntoB.parent = null;
    }

    void Update()
    {
        // Comprobar si el jugador está dentro del rango de visión
        if (Vector2.Distance(transform.position, objetivo.position) <= distanciaVision)
        {
            // Perseguir al jugador
            PerseguirJugador();
        }
        else
        {
            // Patrullar entre los dos puntos
            Patrullar();
        }
    }

    void Patrullar()
    {
        // Determinar la dirección de movimiento
        Vector2 direccion = yendoAPuntoB ? (puntoB.position - transform.position).normalized : (puntoA.position - transform.position).normalized;

        // Mover al enemigo
        transform.Translate(direccion * velocidadPatrulla * Time.deltaTime);

        // Si llega a uno de los puntos, cambiar de dirección
        if (yendoAPuntoB && Vector2.Distance(transform.position, puntoB.position) < 0.1f)
        {
            yendoAPuntoB = false;
        }
        else if (!yendoAPuntoB && Vector2.Distance(transform.position, puntoA.position) < 0.1f)
        {
            yendoAPuntoB = true;
        }
    }

    void PerseguirJugador()
    {
        // Mover hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidadPersecucion * Time.deltaTime);
    }
}

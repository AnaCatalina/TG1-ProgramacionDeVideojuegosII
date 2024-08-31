using UnityEngine;
using System.Collections;

public class Sensor_HeroKnight : MonoBehaviour {

    private int m_ColCount = 0; // Contador de colisiones

    private float m_DisableTimer; // Temporizador de desactivacion

    private void OnEnable()
    {
        m_ColCount = 0; // Restablecer el contador al activar el objeto
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false; // Devolver falso si el temporizador de desactivacion esta activo
        return m_ColCount > 0; // Devolver verdadero si hay colisiones detectadas
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_ColCount++; // Incrementar el contador cuando se detecta una colision
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--; // Decrementar el contador cuando una colision sale del area
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime; // Actualizar el temporizador de desactivacion
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration; // Establecer el temporizador de desactivacion
    }
}
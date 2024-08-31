using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BalaJugador : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigid;
    private GameObject player;
    private Action<BalaJugador> desactivarAccion;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Vector2 direccion = (player.transform.position - transform.position).normalized;
        rigid.velocity = direccion * speed;

        float rot = Mathf.Atan2(-direccion.y, -direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot);

        StartCoroutine("DesactivarTiempo");
    }

    // Update is called once per frame
    IEnumerator DesactivarTiempo()
    {
        yield return new WaitForSeconds(5f);
        desactivarAccion(this);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            coll.transform.GetComponent<Daño>().TomarDano(15f);
            desactivarAccion(this);
            if(coll.transform.GetComponent<Daño>().InformeVida() <= 0)
            {
                AudioManager.instance.Reproducir(4);
                SceneManager.LoadScene("MenuDerrota");
            }
        }else if(coll.CompareTag("Escudo"))
        {
            AudioManager.instance.Reproducir(2);
            print("Bloqueo");
        }
    }

    public void InhabilitarBala(Action<BalaJugador> desactivarAccionParametro)
    {
        desactivarAccion = desactivarAccionParametro;
    }
}

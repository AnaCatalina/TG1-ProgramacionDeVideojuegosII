using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemigoDistancia : MonoBehaviour
{
    public GameObject player;
    public BalaJugador balaPrefab;
    public LayerMask layer;
    private Rigidbody2D rigid;
    public Transform puntoDisparo;
    public Animator anim;
    public float tiempoEspera, tiempoUltimoDisparo, tiempoEntreDisparo;
    public float distancia;
    public bool rango_vision;
    //Balas
    private ObjectPool<BalaJugador> balasPool;

    void Awake()
    {    
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    void OnEnable()
    {
        balasPool = new ObjectPool<BalaJugador>(() => 
        {
            BalaJugador bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
            bala.InhabilitarBala(DesactivarBala);
            return bala;
        }, bala => 
        {
            bala.transform.position = puntoDisparo.position;
            ActivarBala(bala);
        }, bala => 
        {
            bala.gameObject.SetActive(false);
        }, bala => 
        {
            Destroy(bala.gameObject);
        }, true, 10, 25);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rango_vision = Physics2D.Raycast(puntoDisparo.position, transform.right, distancia, layer);
        if(rango_vision)
        {
            GetComponent<MovimientoEnemigo>().enabled = false;
            if(Time.time > (tiempoEntreDisparo + tiempoUltimoDisparo))
            {
                tiempoUltimoDisparo += 4f;
                Invoke(nameof(Disparar), tiempoEspera);
            }
        }else{
            GetComponent<MovimientoEnemigo>().enabled = true;
        }
    }

    private void Disparar()
    {
        anim.SetTrigger("Disparar");
        balasPool.Get();
    }

    private void ActivarBala(BalaJugador bala)
    {
        bala.gameObject.SetActive(true);
    }

    private void DesactivarBala(BalaJugador bala)
    {
        balasPool.Release(bala);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(puntoDisparo.position, puntoDisparo.position + transform.right * distancia);
    }
}

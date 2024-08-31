using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimientoEnemigo : MonoBehaviour
{
    //Player
    public GameObject player;
    public LayerMask layer;
    //Enemigo
    public Transform controlador; 
    private Rigidbody2D rigid;
    public Transform enemy;
    public Transform punto1, punto2;
    public Animator anim;
    public float speed_walk, speed_run;
    public bool rango_vision;
    public float rango_ataque, danioGolpe;
    public float distancia;
    private bool movingRight = true;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    void Start()
    {
        punto1.parent = null;
        punto2.parent = null;
        StartCoroutine("Patrulla");
    }

    IEnumerator Patrulla()
    {
        while (true)
        {
            anim.SetBool("walk", true);
            // Verificar si el jugador está dentro del rango de visión
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, 5f, layer);
            if (playerCollider != null)
            {
                // Calcular la dirección hacia el jugador
                Vector2 directionToPlayer = (playerCollider.transform.position - transform.position).normalized;
                // Si el jugador está dentro del rango de ataque, detenerse y atacar
                if (Vector2.Distance(transform.position, playerCollider.transform.position) <= rango_ataque)
                {
                    rigid.velocity = Vector2.zero;
                    yield return new WaitForSeconds(1);
                    anim.SetBool("attack", true);
                    Golpear();
                }
                else // Si el jugador está dentro del rango de visión pero fuera del rango de ataque, perseguir al jugador
                {
                    rigid.velocity = directionToPlayer * speed_run;
                    if(enemy.position.x > punto1.position.x || enemy.position.x < punto2.position.x)
                    {
                        anim.SetBool("walk", false);
                        rigid.velocity = Vector2.zero;
                    }
                    if (directionToPlayer.x > 0) // Si el jugador está a la derecha, mirar hacia la derecha
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    else // Si el jugador está a la izquierda, mirar hacia la izquierda
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else // Si el jugador no está dentro del rango de visión, continuar patrullando
            {
                // Movimiento de patrulla
                if (movingRight)
                {
                    rigid.velocity = new Vector2(speed_walk, rigid.velocity.y);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (enemy.position.x > punto1.position.x)
                    {
                        anim.SetBool("walk", false);
                        rigid.velocity = Vector2.zero;
                        yield return new WaitForSeconds(2f);
                        movingRight = false;
                    }
                }
                else
                {
                    rigid.velocity = new Vector2(-speed_walk, rigid.velocity.y);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    if (enemy.position.x < punto2.position.x)
                    {
                        anim.SetBool("walk", false);
                        rigid.velocity = Vector2.zero;
                        yield return new WaitForSeconds(2f);
                        movingRight = true;
                    }
                }
            }
            yield return null;
        }
    }

    private void Golpear()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controlador.position, rango_ataque);

        foreach(Collider2D colisionador in objetos)
        {
            Daño dano = colisionador.transform.GetComponent<Daño>();
            if(colisionador.CompareTag("Player"))
            {        
                dano.TomarDano(danioGolpe);
                AudioManager.instance.Reproducir(15);
                if(dano.InformeVida() == 0)
                {
                    AudioManager.instance.Reproducir(4);
                    SceneManager.LoadScene("MenuDerrota");
                }
            }else if(colisionador.CompareTag("Escudo"))
            {
                AudioManager.instance.Reproducir(2);
                print("Bloqueo");
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controlador.position, rango_ataque);
    }
}

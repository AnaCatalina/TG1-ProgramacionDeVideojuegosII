using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrullajeEnemigo : MonoBehaviour
{
    public Transform enemy;
    public Transform point1, point2;
    private Rigidbody2D rb;
    public Animator anim;
    public LayerMask playerLayer;
    public GameObject rango;
    public GameObject hit;
    public bool atacando;
    public float speed;
    public float visionRange = 5f;
    public float attackRange = 1f;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
        point1.parent = null;
        point2.parent = null;
    }
    
    IEnumerator Movement()
    {
        while (true)
        {
            anim.SetBool("walk", true);

            // Verificar si el jugador está dentro del rango de visión
            
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);
            if (playerCollider != null)
            {
                // Calcular la dirección hacia el jugador
                anim.SetBool("walk", false);
                anim.SetBool("run", true);
                Vector2 directionToPlayer = (playerCollider.transform.position - transform.position).normalized;

                // Si el jugador está dentro del rango de ataque, detenerse y atacar
                if (Vector2.Distance(transform.position, playerCollider.transform.position) <= attackRange)
                {
                    anim.SetBool("run", false);
                    rb.velocity = Vector2.zero;
                    anim.SetBool("attack", true);
                }
                else // Si el jugador está dentro del rango de visión pero fuera del rango de ataque, perseguir al jugador
                {
                    rb.velocity = directionToPlayer * speed;
                    if (directionToPlayer.x > 0) // Si el jugador está a la derecha, mirar hacia la derecha
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    else // Si el jugador está a la izquierda, mirar hacia la izquierda
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else // Si el jugador no está dentro del rango de visión, continuar patrullando
            {
                //anim.SetBool("run", false);
                anim.SetBool("walk", true);
                // Movimiento de patrulla
                if (movingRight)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    if (enemy.position.x > point1.position.x)
                    {
                        anim.SetBool("walk", false);
                        rb.velocity = Vector2.zero;
                        yield return new WaitForSeconds(2f);
                        movingRight = false;
                    }
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (enemy.position.x < point2.position.x)
                    {
                        anim.SetBool("walk", false);
                        rb.velocity = Vector2.zero;
                        yield return new WaitForSeconds(2f);
                        movingRight = true;
                    }
                }
            }
            yield return null;
        }
    }

    public void final_Ani()
    {
        anim.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void colliderWeaponTrue()
    {
        hit.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void colliderWeaponFalse()
    {
        hit.GetComponent<BoxCollider2D>().enabled = false;
    }
}

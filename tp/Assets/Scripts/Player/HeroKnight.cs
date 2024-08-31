using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 5.0f; // Velocidad del personaje
    [SerializeField] float      m_jumpForce = 12.0f; // Fuerza de salto del personaje
    [SerializeField] float      m_rollForce = 6.0f; // Fuerza de rodar del personaje
    //[SerializeField] bool       m_noBlood = false; // Booleano que controla si se muestra sangre al morir el personaje
    [SerializeField] private GameObject m_slideDust; // Particulas de polvo al deslizar
    [SerializeField] private Transform controlador;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danioGolpe;

    private Animator            m_animator; // Referencia al componente Animator del personaje
    private Rigidbody2D         m_body2d; // Referencia al componente Rigidbody2D del personaje
    private Sensor_HeroKnight   m_groundSensor; // Referencia al sensor de suelo del personaje
    private Sensor_HeroKnight   m_wallSensorR1; // Referencia al sensor de pared derecha del personaje
    private Sensor_HeroKnight   m_wallSensorR2; // Referencia al segundo sensor de pared derecha del personaje
    private Sensor_HeroKnight   m_wallSensorL1; // Referencia al sensor de pared izquierda del personaje
    private Sensor_HeroKnight   m_wallSensorL2; // Referencia al segundo sensor de pared izquierda del personaje
    private bool                m_isWallSliding = false; // Booleano que indica si el personaje esta deslizando en una pared
    private bool                m_grounded = false; // Booleano que indica si el personaje esta en el suelo
    private bool                m_rolling = false; // Booleano que indica si el personaje esta rodando
    private int                 m_facingDirection = 1; // Direccion hacia la que mira el personaje (1: derecha, -1: izquierda)
    private int                 m_currentAttack = 0; // Numero de ataque actual en el combo
    private float               m_timeSinceAttack = 0.0f; // Tiempo transcurrido desde el ultimo ataque
    private float               m_delayToIdle = 0.0f; // Retraso para volver a la animacion de reposo
    private float               m_rollDuration = 8.0f / 14.0f; // Duracion de la accion rodar en segundos
    private float               m_rollCurrentTime; // Tiempo actual de la accion rodar
    public float tiempoentreAt, tiemposigAt;
    public GameObject escudo;
    public bool m_isAttacking = false, m_isBlocking = false, m_isMoving = false;

    void Start ()
    {
        m_animator = GetComponent<Animator>(); // Obtener el componente Animator del personaje
        m_body2d = GetComponent<Rigidbody2D>(); // Obtener el componente Rigidbody2D del personaje
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>(); // Obtener el sensor de suelo
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>(); // Obtener el sensor de pared derecha 1
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>(); // Obtener el sensor de pared derecha 2
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>(); // Obtener el sensor de pared izquierda 1
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>(); // Obtener el sensor de pared izquierda 2
    }

    // Update is called once per frame
    void Update ()
    {
        // Aumentar el temporizador que controla el combo de ataque
        m_timeSinceAttack += Time.deltaTime;

        // Aumentar el temporizador que comprueba la duracion de la accion rodar
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Desactivar rodar si el temporizador excede la duracion
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // Comprobar si el personaje acaba de aterrizar en el suelo
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Comprobar si el personaje acaba de empezar a caer
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Manejar entrada y movimiento --
        float inputX = Input.GetAxis("Horizontal"); // Obtener la entrada horizontal

        // Cambiar la direccion del sprite dependiendo de la direccion del movimiento
        if (inputX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }else if (inputX < 0){
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Mover
        if (!m_rolling && !m_isAttacking && !m_isBlocking){
            m_body2d.velocity = new Vector2(inputX * m_speed * 3, m_body2d.velocity.y);
            m_isMoving = true;
        }else{
            m_isMoving = false;
        }

        // Establecer la velocidad en el aire en el animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y * 1.5f);

        // -- Manejar animaciones --
        // Deslizar en la pared
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);
        if(Input.GetKeyDown("j") && m_timeSinceAttack > 0.25f && !m_rolling && !m_isBlocking)
        {
            m_body2d.velocity = Vector2.zero;
            AudioManager.instance.Reproducir(5);
            m_currentAttack++;

            // Volver al uno despues del tercer ataque
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Restablecer el combo de ataque si el tiempo desde el ultimo ataque es demasiado grande
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);// Llamar a una de las tres animaciones de ataque "Attack1", "Attack2", "Attack3"
            m_timeSinceAttack = 0.0f;// Restablecer temporizador
            Golpear();
            m_isAttacking = true;
        }else if(Input.GetKeyUp("j") && !m_rolling && !m_isBlocking){
            m_isAttacking = false;
        }
        // Bloquear
        else if (Input.GetKeyDown("k") && !m_rolling && !m_isAttacking)
        {
            m_body2d.velocity = Vector2.zero;
            m_animator.SetTrigger("Block");
            escudo.GetComponent<Collider2D>().enabled = true;
            m_animator.SetBool("IdleBlock", true);
            m_isBlocking = true;
        }

        // Desactivar bloqueo cuando se suelta el boton
        else if (Input.GetKeyUp("k") && !m_rolling && !m_isAttacking){
            escudo.GetComponent<Collider2D>().enabled = false;
            m_animator.SetBool("IdleBlock", false);
            m_isBlocking = false;
        }
        // Rodar
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            AudioManager.instance.Reproducir(6);
        }
        // Saltar
        else if (Input.GetKeyDown("w") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            AudioManager.instance.Reproducir(3);
        }

        // Correr
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reiniciar temporizador
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        // Reposo
        else
        {
            // Evita transiciones parpadeantes a reposo
            m_delayToIdle -= Time.deltaTime;
            if(m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    private void Golpear()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controlador.position, radioGolpe);

        foreach(Collider2D colisionador in objetos)
        {
            if(colisionador.CompareTag("Enemigos"))
            {
                colisionador.transform.GetComponent<Daño>().TomarDano(danioGolpe);
                AudioManager.instance.Reproducir(14);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controlador.position, radioGolpe);
    }

    // Eventos de animacion
    // Llamado en la animacion de deslizamiento.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Establecer posicion de aparicion correcta
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Girar el polvo en la direccion correcta
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
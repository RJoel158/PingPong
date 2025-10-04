using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Puntos que sumara
    public int points = 1;

    //valocidad inicial de la pelota, con la que comenzara
    public float initVelocity = 4f;

    //Mejoras y extras
    //Ajustar la velocidad de la pelota:
    //Auemnta un 10% mas al collisionar con un paddle
    public float velocityMultiplier = 1.1f;

    // Sistema de estela
    public ParticleSystem trailParticles;
    public TrailRenderer trailRenderer;
    public bool enableTrail = true;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //Referencia de riginbody
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTrailEffect();
    }

    void UpdateTrailEffect()
    {
        if (trailParticles != null && enableTrail)
        {
            // Obtener la velocidad actual de la pelota
            Vector2 ballVelocity = rb.velocity;

            // Si la pelota se está moviendo
            if (ballVelocity.magnitude > 0.1f)
            {
                // Calcular la dirección opuesta al movimiento (para la estela)
                Vector2 trailDirection = -ballVelocity.normalized;

                // Configurar la dirección de las partículas
                var velocityOverLifetime = trailParticles.velocityOverLifetime;
                velocityOverLifetime.enabled = true;
                velocityOverLifetime.space = ParticleSystemSimulationSpace.World;

                // Establecer la velocidad de las partículas en dirección opuesta
                velocityOverLifetime.x = trailDirection.x * 2f; // Multiplicador para intensidad
                velocityOverLifetime.y = trailDirection.y * 2f;

                // Activar el sistema de partículas si está pausado
                if (!trailParticles.isPlaying)
                {
                    trailParticles.Play();
                }
            }
            else
            {
                // Pausar partículas si la pelota no se mueve
                if (trailParticles.isPlaying)
                {
                    trailParticles.Pause();
                }
            }
        }
    }

    void Launch()
    {
        //Definir que direccion va tomar la pelota de manera inicial de forma randomica en el eje x - y
        float xVelocity = Random.Range(0, 2) == 1 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 1 ? 1 : -1;

        //Asignar la velocidad inicial a la pelota
        rb.velocity = new Vector2(xVelocity, yVelocity) * initVelocity;

        // Iniciar la estela cuando la pelota se lanza
        if (trailParticles != null && enableTrail)
        {
            trailParticles.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Cuando colisione con una raqueta que tiene el tage de Paddle, multiplicamo su velocidad actual por el valor de la variable velocityMultiplier
        //Por defecto esta en 10% deifnido arriba, totalmente regulable
        if (collision.gameObject.CompareTag("Paddle"))
        {
            rb.velocity = rb.velocity * velocityMultiplier;


        }
    }


    // Método público para detener la pelota
    public void StopBall()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Detener la estela cuando la pelota se detiene
        if (trailParticles != null && trailParticles.isPlaying)
        {
            trailParticles.Stop();
        }

        // Limpiar el trail renderer
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }

    // Método público para reanudar el movimiento de la pelota
    public void ResumeBall()
    {
        if (rb.velocity == Vector2.zero)
        {
            Launch();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si la pelota toca la meta del lado izquierdo
        if (collision.gameObject.CompareTag("GoalLeft"))
        {
            //Aumentar el puntaje del jugador derecho
            GameManager.Instance.AddPaddleRightScore(points);

            //Reiniciar la posici�n de la pelota y las raquetas
            GameManager.Instance.Restart();

            //Lanzar la pelota en una nueva direcci�n
            Launch();
        }

        //Si la pelota toca la meta del lado derecho
        if (collision.gameObject.CompareTag("GoalRight"))
        {
            //Aumentar el puntaje del jugador izquierdo
            GameManager.Instance.AddPaddleLeftScore(points);

            //Reiniciar la posici�n de la pelota y las raquetas
            GameManager.Instance.Restart();

            //Lanzar la pelota en una nueva direcci�n
            Launch();
        }
    }

}

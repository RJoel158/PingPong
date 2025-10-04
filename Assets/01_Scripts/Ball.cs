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
        
    }

    void Launch()
    {
        //Definir que direccion va tomar la pelota de manera inicial de forma randomica en el eje x - y
        float xVelocity = Random.Range(0, 2) == 1 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 1 ? 1 : -1;

        //Asignar la velocidad inicial a la pelota
        rb.velocity = new Vector2(xVelocity, yVelocity) * initVelocity;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si la pelota toca la meta del lado izquierdo
        if (collision.gameObject.CompareTag("GoalLeft"))
        {
            //Aumentar el puntaje del jugador derecho
            GameManager.Instance.AddPaddleLeftScore(points);

            //Reiniciar la posición de la pelota y las raquetas
            GameManager.Instance.Restart();

            //Lanzar la pelota en una nueva dirección
            Launch();
        }

        //Si la pelota toca la meta del lado derecho
        if (collision.gameObject.CompareTag("GoalRight"))
        {
            //Aumentar el puntaje del jugador izquierdo
            GameManager.Instance.AddPaddleRightScore(points);

            //Reiniciar la posición de la pelota y las raquetas
            GameManager.Instance.Restart();

            //Lanzar la pelota en una nueva dirección
            Launch();
        }
    }

}

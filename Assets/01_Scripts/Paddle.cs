using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Rigidbody2D rb;
    //Velocidad de movimiento de las raquetas
    public float moveSpeed = 7f;
    //Maxima distancia que puede recorrer la raqueta en el eje Y, ya sea con + o -
    public float yBound = 3.5f;

    //Indetificador de: si es la raqueta izquierda = true, si es la raqueta derecha = false
    public bool PaddingLeft = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Obtener referencia de rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
        VerifyWinner();
    }

    void BasicMovement()
    {
        float inputMovement;
        if (PaddingLeft)
        {
            //Si es raqueta de izquierda, tonces obtebemos w y s
            inputMovement = Input.GetAxisRaw("VerticalLeft");
        }
        else
        {
            //Sino flecha arriba y abajo
            inputMovement = Input.GetAxisRaw("VerticalRight");
        }

        //Simulacion de donde iria la pelota en el siguiente frame, primero obtener la posicion actual
        Vector2 paddlePosition = transform.position;

        //Mathf.Clamp limita un valor dentro de un rango:
        //- Si el valor est� entre min y max, lo devuelve igual.
        //- Si el valor es menor que min, devuelve min.
        //- Si el valor es mayor que max, devuelve max
        //Limitar que la raqueta salga de rango, definido en yBound
        paddlePosition.y = Mathf.Clamp(paddlePosition.y + inputMovement * moveSpeed * Time.deltaTime, -yBound, yBound);

        //Pasamos ese valor al transform de este gameObject
        transform.position = paddlePosition;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            animator.SetTrigger("hiting");
        }
    }
    void VerifyWinner()
    {
        // Acceder a los puntajes desde GameManager
        if (GameManager.Instance.GetPaddleLeftScore() >= 10)
        {
            Debug.Log("Ganó la raqueta izquierda");
            // Si esta raqueta es la derecha (perdedora), ejecuta animación de muerte
            if (!PaddingLeft)
            {
                ExecuteDeathAnimation();
            }
        }
        else if (GameManager.Instance.GetPaddleRightScore() >= 10)
        {
            Debug.Log("Ganó la raqueta derecha");
            // Si esta raqueta es la izquierda (perdedora), ejecuta animación de muerte
            if (PaddingLeft)
            {
                ExecuteDeathAnimation();
            }
        }
    }

    void ExecuteDeathAnimation()
    {
        // Ejecutar trigger de muerte
        animator.SetTrigger("Die");

        // Desactivar el movimiento para que no interfiera con la animación
        moveSpeed = 0f;

        // Opcional: Desactivar el collider para que no interactúe más con la pelota
        GetComponent<Collider2D>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Variable para controlar si ya se inició el reinicio de escena
    private static bool gameEnded = false;

    // Variable para controlar si ya se ejecutó la animación de muerte en este paddle
    private bool deathAnimationExecuted = false;

    // Start is called before the first frame update
    void Start()
    {
        //Obtener referencia de rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Reiniciar todas las variables para la nueva partida
        deathAnimationExecuted = false;
        gameEnded = false;
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
        // Solo verificar si el juego no ha terminado aún
        if (gameEnded) return;

        // Acceder a los puntajes desde GameManager
        if (GameManager.Instance.GetPaddleLeftScore() >= 1)
        {
            Debug.Log("Ganó la raqueta izquierda");

            // Marcar que el juego ha terminado
            gameEnded = true;

            // Detener la pelota cuando hay un ganador
            StopBallOnWin();

            // Si esta raqueta es la derecha (perdedora), ejecuta animación de muerte INMEDIATAMENTE
            if (!PaddingLeft && !deathAnimationExecuted)
            {
                ExecuteDeathAnimation();
            }

            // Solo el paddle izquierdo (ganador) inicia el reinicio para evitar múltiples corrutinas
            if (PaddingLeft)
            {
                StartCoroutine(RestartSceneAfterDelay(5f));
            }
        }
        else if (GameManager.Instance.GetPaddleRightScore() >= 1)
        {
            Debug.Log("Ganó la raqueta derecha");

            // Marcar que el juego ha terminado
            gameEnded = true;

            // Detener la pelota cuando hay un ganador
            StopBallOnWin();

            // Si esta raqueta es la izquierda (perdedora), ejecuta animación de muerte INMEDIATAMENTE
            if (PaddingLeft && !deathAnimationExecuted)
            {
                ExecuteDeathAnimation();
            }

            // Solo el paddle derecho (ganador) inicia el reinicio para evitar múltiples corrutinas
            if (!PaddingLeft)
            {
                StartCoroutine(RestartSceneAfterDelay(5f));
            }
        }
    }

    void StopBallOnWin()
    {
        // Buscar la pelota en la escena
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.StopBall();
            }
        }
    }

    // Corrutina para reiniciar la escena después de un delay
    IEnumerator RestartSceneAfterDelay(float delay)
    {
        Debug.Log($"La escena se reiniciará en {delay} segundos...");

        // Esperar el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Obtener el nombre de la escena actual y recargarla
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void ExecuteDeathAnimation()
    {
        // Marcar que la animación ya se ejecutó
        deathAnimationExecuted = true;

        // Ejecutar trigger de muerte
        animator.SetTrigger("die");

        // Desactivar el movimiento para que no interfiera con la animación
        moveSpeed = 0f;

        // Opcional: Desactivar el collider para que no interactúe más con la pelota
        GetComponent<Collider2D>().enabled = false;

        Debug.Log($"Animación de muerte ejecutada para paddle: {(PaddingLeft ? "Izquierdo" : "Derecho")}");
    }
}

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
    private bool deathAnimationExecuted = false;    // Start is called before the first frame update
    void Start()
    {
        //Obtener referencia de rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Reiniciar variables para nueva partida
        deathAnimationExecuted = false;
        gameEnded = false;

        Debug.Log($"🔄 NUEVA PARTIDA - Paddle {(PaddingLeft ? "IZQUIERDO" : "DERECHO")} iniciado");
    }    // Update is called once per frame
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
        // Si ya ejecuté mi animación de muerte, no hacer nada más
        if (deathAnimationExecuted) return;

        // Verificar condiciones de victoria
        bool leftWon = GameManager.Instance.GetPaddleLeftScore() >= 1;
        bool rightWon = GameManager.Instance.GetPaddleRightScore() >= 1;

        // Si alguien ganó, procesar INMEDIATAMENTE
        if (leftWon || rightWon)
        {
            Debug.Log($" PADDLE {(PaddingLeft ? "IZQUIERDO" : "DERECHO")} detectó victoria: leftWon={leftWon}, rightWon={rightWon}, gameEnded={gameEnded}");

            // Solo detener la pelota una vez (el primer paddle que detecte)
            if (!gameEnded)
            {
                gameEnded = true;
                StopBallOnWin();
                Debug.Log(" Pelota detenida por primera vez");
            }

            // Verificar si ESTE paddle debe ejecutar animación de muerte
            bool shouldDie = (PaddingLeft && rightWon) || (!PaddingLeft && leftWon);

            if (shouldDie && !deathAnimationExecuted)
            {
                Debug.Log($" PADDLE {(PaddingLeft ? "IZQUIERDO" : "DERECHO")} perdió - Ejecutando animación");
                ExecuteDeathAnimation();

                // Solo el perdedor inicia el reinicio
                StartCoroutine(RestartSceneAfterDelay(5f));
            }
            else if (!shouldDie)
            {
                Debug.Log($" PADDLE {(PaddingLeft ? "IZQUIERDO" : "DERECHO")} ganó - Sin animación");
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
        Debug.Log($" INICIANDO ExecuteDeathAnimation para paddle: {(PaddingLeft ? "Izquierdo" : "Derecho")}");

        // Marcar que la animación ya se ejecutó
        deathAnimationExecuted = true;

        // Ejecutar trigger de muerte
        if (animator != null)
        {
            animator.SetTrigger("die");
            Debug.Log($" Trigger 'die' enviado al animator del paddle {(PaddingLeft ? "Izquierdo" : "Derecho")}");
        }
        else
        {
            Debug.LogError($" Animator es NULL en paddle {(PaddingLeft ? "Izquierdo" : "Derecho")}");
        }

        // Desactivar el movimiento para que no interfiera con la animación
        moveSpeed = 0f;

        // Opcional: Desactivar el collider para que no interactúe más con la pelota
        GetComponent<Collider2D>().enabled = false;

        Debug.Log($" Animación de muerte completada para paddle: {(PaddingLeft ? "Izquierdo" : "Derecho")}");
    }
}

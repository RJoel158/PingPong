using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paddle : MonoBehaviour
{
    Rigidbody2D rb;
    // Movement speed of the paddles
    public float moveSpeed = 7f;
    // Maximum distance the paddle can travel on the Y axis, either positive or negative
    public float yBound = 3.5f;

    // Identifier: if it's the left paddle = true, if it's the right paddle = false
    public bool PaddingLeft = false;

    public Animator animator;

    // Variable to control if scene restart has already been initiated
    private static bool gameEnded = false;

    // Variable to control if death animation has already been executed on this paddle
    private bool deathAnimationExecuted = false;    // Start is called before the first frame update
    void Start()
    {
        // Get rigidbody reference
        rb = GetComponent<Rigidbody2D>();

        // Get Animator reference if not manually assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                Debug.Log($"Animator found automatically on {(PaddingLeft ? "Left Paddle" : "Right Paddle")}");
            }
            else
            {
                Debug.LogWarning($"No Animator found on {(PaddingLeft ? "Left Paddle" : "Right Paddle")}");
            }
        }

        // Reset variables for new game
        deathAnimationExecuted = false;
        gameEnded = false;

        Debug.Log($"NEW GAME - Paddle {(PaddingLeft ? "LEFT" : "RIGHT")} initialized");
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
            // If it's the left paddle, then we get W and S keys
            inputMovement = Input.GetAxisRaw("VerticalLeft");
        }
        else
        {
            // Otherwise up and down arrow keys
            inputMovement = Input.GetAxisRaw("VerticalRight");
        }

        // Simulation of where the paddle would go in the next frame, first get current position
        Vector2 paddlePosition = transform.position;

        // Mathf.Clamp limits a value within a range:
        // - If the value is between min and max, returns it as is
        // - If the value is less than min, returns min
        // - If the value is greater than max, returns max
        // Limit the paddle from going out of range, defined by yBound
        paddlePosition.y = Mathf.Clamp(paddlePosition.y + inputMovement * moveSpeed * Time.deltaTime, -yBound, yBound);

        // Pass that value to the transform of this gameObject
        transform.position = paddlePosition;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Only try animation if animator exists
            if (animator != null)
            {
                animator.SetTrigger("hiting");
            }
            else
            {
                Debug.LogWarning($"Animator not configured on {(PaddingLeft ? "Left Paddle" : "Right Paddle")}");
            }
        }
    }
    void VerifyWinner()
    {
        // If I already executed my death animation, do nothing more
        if (deathAnimationExecuted) return;

        // Check victory conditions
        bool leftWon = GameManager.Instance.GetPaddleLeftScore() >= 10;
        bool rightWon = GameManager.Instance.GetPaddleRightScore() >= 10;

        // If someone won, process IMMEDIATELY
        if (leftWon || rightWon)
        {
            Debug.Log($"PADDLE {(PaddingLeft ? "LEFT" : "RIGHT")} detected victory: leftWon={leftWon}, rightWon={rightWon}, gameEnded={gameEnded}");

            // Only stop the ball once (the first paddle that detects)
            if (!gameEnded)
            {
                gameEnded = true;
                StopBallOnWin();
                Debug.Log("Ball stopped for the first time");
            }

            // Check if THIS paddle should execute death animation
            bool shouldDie = (PaddingLeft && rightWon) || (!PaddingLeft && leftWon);

            if (shouldDie && !deathAnimationExecuted)
            {
                Debug.Log($"PADDLE {(PaddingLeft ? "LEFT" : "RIGHT")} lost - Executing animation");
                ExecuteDeathAnimation();

                // Only the loser initiates the return to main menu
                StartCoroutine(ReturnToMainMenuAfterDelay(5f));
            }
            else if (!shouldDie)
            {
                Debug.Log($"PADDLE {(PaddingLeft ? "LEFT" : "RIGHT")} won - No animation");
            }
        }
    }
    void StopBallOnWin()
    {
        // Find the ball in the scene
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

    // Coroutine to return to main menu after a delay
    IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        Debug.Log($"Returning to Main Menu in {delay} seconds...");

        // Wait for the specified time
        yield return new WaitForSeconds(delay);

        // Load the MainMenu scene
        Debug.Log("Loading MainMenu scene...");
        SceneManager.LoadScene("MainMenu");
    }

    void ExecuteDeathAnimation()
    {
        Debug.Log($"STARTING ExecuteDeathAnimation for paddle: {(PaddingLeft ? "Left" : "Right")}");

        // Mark that the animation has already been executed
        deathAnimationExecuted = true;

        // Execute death trigger
        if (animator != null)
        {
            animator.SetTrigger("die");
            Debug.Log($"Trigger 'die' sent to animator of paddle {(PaddingLeft ? "Left" : "Right")}");
        }
        else
        {
            Debug.LogError($"Animator is NULL on paddle {(PaddingLeft ? "Left" : "Right")}");
        }

        // Disable movement so it doesn't interfere with animation
        moveSpeed = 0f;

        // Optional: Disable collider so it no longer interacts with the ball
        GetComponent<Collider2D>().enabled = false;

        Debug.Log($"Death animation completed for paddle: {(PaddingLeft ? "Left" : "Right")}");
    }
}

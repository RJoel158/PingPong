using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Points that will be added
    public int points = 1;

    // Initial velocity of the ball, with which it will start
    public float initVelocity = 4f;

    // Improvements and extras
    // Adjust the ball velocity:
    // Increases 10% more when colliding with a paddle
    public float velocityMultiplier = 1.1f;


    // Trail system

    Rigidbody2D rb;

    // Variables for sound when colliding with the ball
    public AudioClip hitSound; // Sound at the moment of collision
    private AudioSource audioSource;

    // Variables for sound when a player loses
    public AudioClip gameOverSound;


    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody reference
        rb = GetComponent<Rigidbody2D>();

        Launch();

        // Call to get the audiosource on the same object
        audioSource = GetComponent<AudioSource>();

        // If there's no AudioSource, create one automatically
        if (audioSource == null)
        {
            Debug.Log("Creating AudioSource automatically for Ball");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // Don't play on start
            audioSource.volume = 1.0f; // Full volume
        }

        // Debug to verify that AudioSource and clips are configured
        Debug.Log($"AudioSource found: {audioSource != null}");
        Debug.Log($"HitSound assigned: {hitSound != null}");
        Debug.Log($"GameOverSound assigned: {gameOverSound != null}");
    }



    // Update is called once per frame
    void Update()
    {
        HandleBallFlip();
    }

    void HandleBallFlip()
    {
        // Check the ball direction
        if (rb.velocity.x < 0) // Moving to the left
        {
            // Flip on X axis (negative scale)
            transform.localScale = new Vector3(-1.7f, 1.7f, 1f);
        }
        else if (rb.velocity.x > 0) // Moving to the right
        {
            // Normal scale
            transform.localScale = new Vector3(1.7f, 1.7f, 1f);
        }
        // If rb.velocity.x == 0, maintain current scale
    }



    void Launch()
    {
        // Define which direction the ball will take initially randomly on the x and y axis
        float xVelocity = Random.Range(0, 2) == 1 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 1 ? 1 : -1;

        // Assign initial velocity to the ball
        rb.velocity = new Vector2(xVelocity, yVelocity) * initVelocity;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When colliding with a paddle that has the Paddle tag, multiply its current velocity by the velocityMultiplier variable value
        // By default it's at 10% defined above, totally adjustable
        if (collision.gameObject.CompareTag("Paddle"))
        {
            rb.velocity = rb.velocity * velocityMultiplier;
            // Instantiate bounce sound with the paddle

            // I put it here so when it collides with any paddle it sounds
            // PlayOneShot is to play something specific once even if there are other sounds
            // hitsound is the reference to the audio file
            if (audioSource != null && hitSound != null)
            {
                Debug.Log("Playing hit sound");
                audioSource.PlayOneShot(hitSound);
            }
            else
            {
                Debug.LogWarning($"Cannot play sound: audioSource={audioSource != null}, hitSound={hitSound != null}");
            }

        }
    }


    // Public method to stop the ball
    public void StopBall()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

    }

    // Public method to resume ball movement
    public void ResumeBall()
    {
        if (rb.velocity == Vector2.zero)
        {
            Launch();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the ball touches the left side goal
        if (collision.gameObject.CompareTag("GoalLeft"))
        {
            // Increase right player score
            GameManager.Instance.AddPaddleRightScore(points);

            // Restart ball and paddle positions

            // Play gameover sound when scoring goal
            if (audioSource != null && gameOverSound != null)
            {
                Debug.Log("Playing game over sound (right player scores)");
                audioSource.PlayOneShot(gameOverSound);
            }
            else
            {
                Debug.LogWarning($"Cannot play game over sound: audioSource={audioSource != null}, gameOverSound={gameOverSound != null}");
            }

            // Start explosion effect with fade

            GameManager.Instance.Restart();

            // Launch the ball in a new direction
            Launch();
        }

        // If the ball touches the right side goal
        if (collision.gameObject.CompareTag("GoalRight"))
        {
            // Increase left player score
            GameManager.Instance.AddPaddleLeftScore(points);

            // Restart ball and paddle positions


            // Instantiate explosion sprite at ball position

            // Play gameover sound when scoring goal
            if (audioSource != null && gameOverSound != null)
            {
                Debug.Log("Playing game over sound (left player scores)");
                audioSource.PlayOneShot(gameOverSound);
            }
            else
            {
                Debug.LogWarning($"Cannot play game over sound: audioSource={audioSource != null}, gameOverSound={gameOverSound != null}");
            }
            // Start explosion effect with fade

            GameManager.Instance.Restart();

            // Launch the ball in a new direction
            Launch();
        }
    }

}

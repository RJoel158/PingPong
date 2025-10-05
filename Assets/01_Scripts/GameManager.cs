using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Score texts
    public TMP_Text txtPaddleLeftScore;
    public TMP_Text txtPaddleRightScore;

    // Reference to paddles and ball
    public Transform paddleLeft;
    public Transform paddleRight;
    public Transform ball;

    // Variables that will hold the score
    int paddleLeftScore = 0;
    int paddleRightScore = 0;

    // Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction when changing scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to add points to the left side variable, and display on screen through its corresponding text
    public void AddPaddleLeftScore(int points)
    {
        paddleLeftScore += points;
        txtPaddleLeftScore.text = paddleLeftScore.ToString();
    }

    // Method to add points to the right side variable, and display on screen through its corresponding text
    public void AddPaddleRightScore(int points)
    {
        paddleRightScore += points;
        txtPaddleRightScore.text = paddleRightScore.ToString();
    }

    // Method that resets to initial positions, will be called after adding a point on either side
    public void Restart()
    {
        paddleLeft.position = new Vector2(paddleLeft.position.x, 0);
        paddleRight.position = new Vector2(paddleRight.position.x, 0);
        ball.position = new Vector2(0, 0);
    }

    // Methods to get scores from other scripts
    public int GetPaddleLeftScore()
    {
        return paddleLeftScore;
    }

    public int GetPaddleRightScore()
    {
        return paddleRightScore;
    }

}

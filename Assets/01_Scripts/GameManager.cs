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
        Debug.Log("GameManager Start - Fresh instance created");

        // Initialize scores to zero for new game
        paddleLeftScore = 0;
        paddleRightScore = 0;

        // Update UI immediately if references are already set
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to add points to the left side variable, and display on screen through its corresponding text
    public void AddPaddleLeftScore(int points)
    {
        paddleLeftScore += points;
        if (txtPaddleLeftScore != null)
            txtPaddleLeftScore.text = paddleLeftScore.ToString();
        else
            Debug.LogWarning("txtPaddleLeftScore UI reference not found");
    }

    // Method to add points to the right side variable, and display on screen through its corresponding text
    public void AddPaddleRightScore(int points)
    {
        paddleRightScore += points;
        if (txtPaddleRightScore != null)
            txtPaddleRightScore.text = paddleRightScore.ToString();
        else
            Debug.LogWarning("txtPaddleRightScore UI reference not found");
    }

    // Method that resets to initial positions, will be called after adding a point on either side
    public void Restart()
    {
        // Check if transforms exist before trying to access them
        if (paddleLeft != null)
            paddleLeft.position = new Vector2(paddleLeft.position.x, 0);
        if (paddleRight != null)
            paddleRight.position = new Vector2(paddleRight.position.x, 0);
        if (ball != null)
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

    // Method to completely reset the game to initial state (used during gameplay, not for fresh starts)
    public void ResetGame()
    {
        Debug.Log("Resetting game to initial state...");

        // Reset scores to zero
        paddleLeftScore = 0;
        paddleRightScore = 0;

        // Update UI
        UpdateScoreUI();

        // Reset positions if objects exist
        Restart();

        // Reset static variables from Paddle class
        ResetPaddleStaticVariables();
    }

    // Method to reset static variables from Paddle class
    private void ResetPaddleStaticVariables()
    {
        // Reset static variables
        Paddle.ResetStaticVariables();

        // Find all paddles in the scene and reset their individual variables
        // Only if there are paddles in the current scene
        Paddle[] paddles = FindObjectsOfType<Paddle>();
        if (paddles != null && paddles.Length > 0)
        {
            foreach (Paddle paddle in paddles)
            {
                if (paddle != null)
                {
                    paddle.ResetPaddleState();
                }
            }
        }
        else
        {
            Debug.Log("No paddles found in current scene - skipping paddle reset");
        }
    }

    // Method to refresh references when entering a new scene
    public void RefreshSceneReferences()
    {
        Debug.Log("Refreshing GameManager scene references...");

        // Try to find the UI elements in the new scene
        if (txtPaddleLeftScore == null)
        {
            GameObject leftScoreObj = GameObject.Find("txtPaddleLeftScore");
            if (leftScoreObj != null)
                txtPaddleLeftScore = leftScoreObj.GetComponent<TMP_Text>();
        }

        if (txtPaddleRightScore == null)
        {
            GameObject rightScoreObj = GameObject.Find("txtPaddleRightScore");
            if (rightScoreObj != null)
                txtPaddleRightScore = rightScoreObj.GetComponent<TMP_Text>();
        }

        // Try to find the game objects in the new scene
        if (paddleLeft == null)
        {
            GameObject paddleLeftObj = GameObject.Find("PaddleLeft");
            if (paddleLeftObj != null)
                paddleLeft = paddleLeftObj.transform;
        }

        if (paddleRight == null)
        {
            GameObject paddleRightObj = GameObject.Find("PaddleRight");
            if (paddleRightObj != null)
                paddleRight = paddleRightObj.transform;
        }

        if (ball == null)
        {
            GameObject ballObj = GameObject.Find("Ball");
            if (ballObj != null)
                ball = ballObj.transform;
        }

        // Update UI with current scores
        UpdateScoreUI();
    }

    // Helper method to update score UI
    private void UpdateScoreUI()
    {
        if (txtPaddleLeftScore != null)
            txtPaddleLeftScore.text = paddleLeftScore.ToString();
        if (txtPaddleRightScore != null)
            txtPaddleRightScore.text = paddleRightScore.ToString();
    }

}

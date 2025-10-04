using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text txtPaddleLeftScore;
    public TMP_Text txtPaddleRightScore;

    public Transform paddleLeft;
    public Transform paddleRight;
    public Transform ball;

    int paddleLeftScore = 0;
    int paddleRightScore = 0;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //Hace que no se destruya al cambiar de escena
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

    public void AddPaddleLeftScore(int points)
    {
        paddleLeftScore += points;
        txtPaddleLeftScore.text = paddleLeftScore.ToString();
    }

    public void AddPaddleRightScore(int points)
    {
        paddleRightScore += points;
        txtPaddleRightScore.text = paddleRightScore.ToString();
    }

    public void Restart()
    {
        paddleLeft.position = new Vector2(paddleLeft.position.x, 0);
        paddleRight.position = new Vector2(paddleRight.position.x, 0);
        ball.position = new Vector2(0, 0);
    }
}

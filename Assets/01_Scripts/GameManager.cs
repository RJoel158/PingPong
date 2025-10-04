using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //textos del puntaje
    public TMP_Text txtPaddleLeftScore;
    public TMP_Text txtPaddleRightScore;

    //Referencia de las raquetas y pelota
    public Transform paddleLeft;
    public Transform paddleRight;
    public Transform ball;

    //variables que tendran el puntaje
    int paddleLeftScore = 0;
    int paddleRightScore = 0;

    //Singleton
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

    //Metodo para sumar puntos a la variable del lado izquierdo, y mostrar en pantalla atraves de su correspondiente texto
    public void AddPaddleLeftScore(int points)
    {
        paddleLeftScore += points;
        txtPaddleLeftScore.text = paddleLeftScore.ToString();
    }

    //Metodo para sumar puntos a la variable del lado derecho, y mostrar en pantalla atraves de su correspondiente texto
    public void AddPaddleRightScore(int points)
    {
        paddleRightScore += points;
        txtPaddleRightScore.text = paddleRightScore.ToString();
    }

    //Metodo que reinicia a la posiciones iniciales, se llamara desde de sumar un punto cualquier lado
    public void Restart()
    {
        paddleLeft.position = new Vector2(paddleLeft.position.x, 0);
        paddleRight.position = new Vector2(paddleRight.position.x, 0);
        ball.position = new Vector2(0, 0);
    }
}

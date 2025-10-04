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
        //- Si el valor está entre min y max, lo devuelve igual.
        //- Si el valor es menor que min, devuelve min.
        //- Si el valor es mayor que max, devuelve max
        //Limitar que la raqueta salga de rango, definido en yBound
        paddlePosition.y = Mathf.Clamp(paddlePosition.y + inputMovement * moveSpeed * Time.deltaTime, -yBound, yBound);

        //Pasamos ese valor al transform de este gameObject
        transform.position = paddlePosition;
    }
}

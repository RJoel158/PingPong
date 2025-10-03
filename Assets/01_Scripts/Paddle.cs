using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 7f;
    public float yBound = 3.5f;

    public bool PaddingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
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
            inputMovement = Input.GetAxisRaw("VerticalLeft");
        }
        else
        {
            inputMovement = Input.GetAxisRaw("VerticalRight");
        }
        Vector2 paddlePosition = transform.position;
        paddlePosition.y = Mathf.Clamp(paddlePosition.y + inputMovement * moveSpeed * Time.deltaTime, -yBound, yBound);

        transform.position = paddlePosition;
    }
}

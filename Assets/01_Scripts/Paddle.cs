using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 7f;

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
        float movement;
        if (PaddingLeft)
        {
            movement = Input.GetAxisRaw("VerticalLeft");
        }
        else
        {
            movement = Input.GetAxisRaw("VerticalRight");
        }

        rb.velocity = new Vector2(0, movement * moveSpeed);
    }
}

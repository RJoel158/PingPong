using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initVelocity = 4f;

    //Auemnta un 10% mas al collisionar con un paddle
    public float velocityMultiplier = 1.1f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Launch()
    {
        float xVelocity = Random.Range(0, 2) == 1 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 1 ? 1 : -1;

        rb.velocity = new Vector2(xVelocity, yVelocity) * initVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            rb.velocity = rb.velocity * velocityMultiplier;
        }
    }
}

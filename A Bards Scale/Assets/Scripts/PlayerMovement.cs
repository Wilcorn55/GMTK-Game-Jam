using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizonalInput;
    float moveSpeed = 10f;
    bool isFacingRight = false;

    public Vector2 lastMortionVector;
    public float jumpPower = 4f;
    bool isJumping = false;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizonalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizonalInput * moveSpeed, rb.velocity.y);
    }

    void FlipSprite() {

        if (isFacingRight && horizonalInput < 0f || !isFacingRight && horizonalInput > 0f)
        {

            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
}

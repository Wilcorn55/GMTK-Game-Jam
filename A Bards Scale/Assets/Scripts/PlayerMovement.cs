using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    Animator animator;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpSpeed;
    private BoxCollider2D boxCollider;
    
    

    public Vector2 lastMortionVector;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * MovementSpeed, body.velocity.y);

        Vector3 scale = transform.localScale;

        if (horizontalInput > 0)
        {
            scale.x = Mathf.Abs(scale.x);  // Ensure the player is facing right
        }
        else if (horizontalInput < 0)
        {
            scale.x = -Mathf.Abs(scale.x);  // Ensure the player is facing left
        }

        transform.localScale = scale;  // Apply the new scale

        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        

        if (Input.GetButtonDown("Jump") && isGrounded()) 
        {
            Jump();
        }
        

    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, JumpSpeed);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    private bool isGrounded() 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}

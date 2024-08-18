using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpSpeed;
    private BoxCollider2D boxCollider;
    

    public Vector2 lastMortionVector;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * MovementSpeed, body.velocity.y);
        
        if(horizontalInput > 0.01f) 
        {
            transform.localScale = Vector3.one;
        }
        else if(horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
                

        

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

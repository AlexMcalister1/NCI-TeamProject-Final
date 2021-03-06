using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingLeft = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 5.0f; 


    void Update()

    {
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);

        if(Input.GetButtonDown("Jump") && onGround)
        {
            Jump();
        }
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void FixedUpdate()
    {
        moveCharacter(direction.x);
        modifyPhysics();
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingLeft) || (horizontal < 0 && facingLeft))
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        }

    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }

     void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;


            }
            else
            {

                rb.drag = 0f;
            }
            rb.gravityScale = 0f;

        }
        else
            rb.gravityScale = gravity;
        rb.drag = linearDrag * 0.15f;
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravity * fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = gravity * (fallMultiplier / 2);
        }
        {

        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        transform.rotation = Quaternion.Euler(0, facingLeft ? 0 : 180, 0);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);

    }
}


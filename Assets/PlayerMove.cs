using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float horizontalSpeed;
    public float jumpSpeed;
    private Rigidbody2D rb;
    private int horizontalInput;

    public LayerMask mask;
    private bool isGrounded;

    private float groundCheckDist = 0.5f;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;
    
    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        checkGrounded();
        if(isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }


    void FixedUpdate()
    {
        horizontalInput = getHorizontalInput();
        rb.velocity = new Vector2(horizontalSpeed * horizontalInput, rb.velocity.y);

    }

    void checkGrounded ()
    {
        RaycastHit2D leftGC = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDist, mask);
        RaycastHit2D rightGC = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDist, mask);

        
        if(leftGC.point != Vector2.zero && rightGC.point != Vector2.zero)
        {
            float res = leftGC.point.y - rightGC.point.y;
            if(Mathf.Abs(res) >= 0.05f && Mathf.Abs(res) < 0.9f)
            {
                Debug.Log("yo");
              //  rb.velocity = new Vector2(Mathf.Sign(res) * 5 + rb.velocity.x, rb.velocity.y);
            }
        }
        
        

        if( (leftGC.collider != null && leftGC.collider.tag == "Ground") || 
        (rightGC.collider != null && rightGC.collider.tag == "Ground"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    int getHorizontalInput()
    {
        if(Input.GetKey(KeyCode.A))
        {
            return -1;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(leftGroundCheck.position, Vector2.down * groundCheckDist);
        Gizmos.DrawRay(rightGroundCheck.position, Vector2.down * groundCheckDist);
    }

}

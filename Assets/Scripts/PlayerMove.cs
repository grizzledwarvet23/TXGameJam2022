using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float horizontalSpeed;
    public float jumpSpeed;
    private Rigidbody2D rb;
    private int horizontalInput;

    public LayerMask groundMask;
    private bool isGrounded;

    //check you aren't stuck in a toggleable wall
    public Transform sideCheck;

    private float groundCheckDist = 0.5f;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;

    private bool facingRight;
    private bool slippery = false;
    public float speedCap = 15;


    public Vector2 spawnPosition;

    //wall jump stuff
    [Header("Wall Jump")]
    public LayerMask wallJumpMask;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.3f;
    bool isWallSliding = false;
    bool enteredWall = false;
    RaycastHit2D wallCheckHit;
    float jumpTime;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        checkGrounded();
        SlopeCheck();
        checkInsideWall();
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.W) && (isGrounded || (isWallSliding && enteredWall) ))
        {
            enteredWall = false;
            if(isWallSliding)
            {
                rb.AddForce(Vector2.up * (jumpSpeed + 3), ForceMode2D.Impulse);    
            }
            else
            {
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
            animator.Play("player_jump");
        }
    }


    void FixedUpdate()
    {
        horizontalInput = getHorizontalInput();

        if(!slippery)
        {
            rb.velocity = new Vector2(horizontalSpeed * horizontalInput, rb.velocity.y);
        }
        else
        {
            rb.AddForce(new Vector2(0.1f * horizontalInput * horizontalSpeed, 0), ForceMode2D.Impulse);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speedCap, speedCap), rb.velocity.y);
        }
        
        if(horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if(horizontalInput < 0 && facingRight)
        {
            Flip();
        }


        //wall jumpy
        if(facingRight)
        {
            wallCheckHit = Physics2D.Raycast(transform.position,
            new Vector2(wallDistance, 0), wallDistance, wallJumpMask);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
        }
        else
        {
            wallCheckHit = Physics2D.Raycast(transform.position,
            new Vector2(-wallDistance, 0), wallDistance, wallJumpMask);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.red);
        }

        animator.SetBool("isWallSliding", false);
        if(enteredWall && wallCheckHit && wallCheckHit.collider.tag == "WallJumpable" && !isGrounded && horizontalInput!=0)
        {
            isWallSliding = true;
            jumpTime = Time.timeSinceLevelLoad + wallJumpTime;  
            animator.SetBool("isWallSliding", true);
        }
    
        else if(jumpTime < Time.timeSinceLevelLoad)
        {
            isWallSliding = false;
        }
        
        

        if(isWallSliding && enteredWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "WallJumpable")
        {
            Debug.Log("Entered");
            enteredWall = true;
        }
    }
    

    void SlopeCheck()
    {
        float colliderSizeY = GetComponent<BoxCollider2D>().size.y;
        Vector2 checkPos = transform.position - new Vector3(0.0f, (colliderSizeY / 2) - 2);

        SlopeCheckVertical(checkPos);
    }

    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 1.0f, groundMask);
        if(hit)
        {
            Vector2 slopeNormalPerp = Vector2.Perpendicular(hit.normal);
            float slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            

            if(slopeNormalPerp.x < -0.1f && slopeNormalPerp.y < -0.1f)
            {
                isGrounded = true;
                rb.velocity = new Vector2(-10, rb.velocity.y);
            }
            if(slopeNormalPerp.x < -0.1f && slopeNormalPerp.y > 0.1f)
            {
                isGrounded = true;
                rb.velocity = new Vector2(10, rb.velocity.y);
            }

            //slide down in the direction of the slope
            //find out if we are on the left or right side of the slope
           // Debug.Log(slopeNormalPerp);
            


        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void checkInsideWall()
    {
        RaycastHit2D sideHit = Physics2D.Raycast(sideCheck.position, Vector2.right, 0.2f, groundMask);
        if(sideHit)
        {
            transform.position = new Vector3(transform.position.x - 1, 
            transform.position.y, transform.position.z);
        }
    }

    void checkGrounded ()
    {
        RaycastHit2D leftGC = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, 
        groundCheckDist, groundMask);
        RaycastHit2D rightGC = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, 
        groundCheckDist, groundMask);



        
        if(leftGC.point != Vector2.zero && rightGC.point != Vector2.zero)
        {
            float res = leftGC.point.y - rightGC.point.y;
            if(Mathf.Abs(res) >= 0.05f && Mathf.Abs(res) < 0.9f)
            {
              //  rb.velocity = new Vector2(Mathf.Sign(res) * 5 + rb.velocity.x, rb.velocity.y);
            }
        }
        
    
        if( (leftGC.collider != null /* && leftGC.collider.tag == "Ground"*/) || 
        (rightGC.collider != null /* && rightGC.collider.tag == "Ground"*/))
        {
            isGrounded = true;
            if(leftGC.collider != null && leftGC.collider.tag == "Slippery" || 
            rightGC.collider != null && rightGC.collider.tag == "Slippery")
            {
                slippery = true;
            }
            else
            {
                slippery = false;
            }
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
        Gizmos.DrawRay(sideCheck.position, Vector2.right * 0.2f);
    }

    

}

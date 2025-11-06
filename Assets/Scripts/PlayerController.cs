using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 5f;
    public int playerHealth  = 5;
    public Rigidbody2D rb;

    public InputAction playerMovement;
    public InputAction jump;
    public InputAction attack;
    public InputAction pause;
    Vector2 moveDir = Vector2.zero;
    private float moveX;
    private float moveY;
    private int jumpsRemaining;
    private float jumpForce;
    private bool jumpTriggered;
    private bool grounded;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    

    private void OnEnable()
    {
        playerMovement.Enable();
        jump.Enable();
        attack.Enable();
        pause.Enable();
    }
    private void OnDisable()
    {
        playerMovement.Disable();
        jump.Disable();
        attack.Disable();
        pause.Disable();
    }


    void Awake()
    {
        jumpForce = 6f;
        jump.performed += onJumpPerformed;
        attack.performed += onAttackPerformed;
    }

    void Update()
    {
        //Movement Vector Read
        moveDir = playerMovement.ReadValue<Vector2>();
        moveX = moveDir.x;
        moveY = moveDir.y;


        if (moveX > 0)
        {
            //spriteRenderer.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetFloat("playerSpeed", moveX);
        }
        else if (moveX < 0)
        {
            //spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetFloat("playerSpeed", Mathf.Abs(moveX));
        }
        else
        {
            animator.SetFloat("playerSpeed", 0);
        }

    }

    void FixedUpdate()
    {
        //Movement Vector Application
        rb.linearVelocity = new Vector2(moveX * playerSpeed * GameController.gcInstance.gameTime,
                                        rb.linearVelocityY * GameController.gcInstance.gameTime);

        //GroundCheck & Jump Logic
        CheckGround();
        if (grounded && rb.linearVelocityY == 0)
        {
            jumpsRemaining = 2;
            animator.SetBool("isJumping", false);
        }

    }

    //Function handles jump input action
    void onJumpPerformed(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            //Debug.Log("Entered Jump: JR=" + jumpsRemaining + " jumpForce=" + jumpForce);
            --jumpsRemaining;
            //Resetting Velocity to add better feeling second jump
            rb.linearVelocityY = 0;
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            //Animation
            animator.SetBool("isJumping", true);
            animator.SetTrigger("jumpTrigger");
        }
    }

    void onAttackPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger("attackTrigger");
    }

    //To handle collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //Handles event subscription, good practice
    void OnDestroy()
    {
        jump.performed -= onJumpPerformed;
        attack.performed -= onAttackPerformed;

    }

    //GroundCheck, Citation Youtube Vid:
    void CheckGround()
    {
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundMask) != null; 
    }
}

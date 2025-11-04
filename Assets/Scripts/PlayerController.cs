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
        jumpForce = 7f;
        
    }

    void Update()
    {
        //Movement Vector Read
        moveDir = playerMovement.ReadValue<Vector2>();
        moveX = moveDir.x;
        moveY = moveDir.y;

        //if(jump.triggered){ jumpTriggered = true; }

    }

    void FixedUpdate()
    {
        //Movement Vector Application
        rb.linearVelocity = new Vector2(moveX * playerSpeed * GameController.gcInstance.gameTime,
                                        rb.linearVelocityY * GameController.gcInstance.gameTime);

        //GroundCheck & Jump Logic
        CheckGround();
        if(grounded){
            jumpsRemaining = 2;
            Debug.Log("grounded");
            }

        if (jump.triggered && (jumpsRemaining > 0))
        {
            --jumpsRemaining;
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            jumpTriggered = false;
            
        }
    }
    


    //GroundCheck, Citation Youtube Vid:
    void CheckGround()
    {
        grounded = Physics2D.OverlapArea(groundCheck.bounds.min, groundCheck.bounds.max, groundMask) != null;
    }
}

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;
    public float playerSpeed = 5f;
    public int playerHealth  = 5;
    public Rigidbody2D rb;

    public InputAction playerMovement;
    public InputAction jump;
    public InputAction attack;
    public InputAction pause;
    public InputAction save;
    public bool canSave;
    Vector2 moveDir = Vector2.zero;
    private float moveX;
    private float moveY;
    private int jumpsRemaining;
    public float jumpForce;
    private bool jumpTriggered;
    private bool grounded;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BoxCollider2D swordCollider;
    public BoxCollider2D playerCollider;

    public UIController uiController;
    public Color flashColor = Color.red;
    public Color originalColor;
    public bool isFacingRight;
    private Vector2 originalPlayerOffset;
    private Vector2 originalSwordOffset;
    public GameObject boss;
    public bool isDead;

    private void OnEnable()
    {
        playerMovement.Enable();
        jump.Enable();
        attack.Enable();
        pause.Enable();
        save.Enable();
    }
    private void OnDisable()
    {
        playerMovement.Disable();
        jump.Disable();
        attack.Disable();
        pause.Disable();
        save.Disable();
    }


    void Awake()
    {
        jump.performed += onJumpPerformed;
        attack.performed += onAttackPerformed;
        save.performed += onSavePerformed;
        pause.performed += onPausePerformed;
        originalColor = spriteRenderer.color;
        canSave = false;
        isFacingRight = true;
        originalPlayerOffset = playerCollider.offset;
        originalSwordOffset = swordCollider.offset;
        swordCollider.enabled = false;
        boss.SetActive(false);
        isDead = false;
    }

    void Update()
    {
        if (!isDead)
        {
            //Movement Vector Read
            moveDir = playerMovement.ReadValue<Vector2>();
            moveX = moveDir.x;
            moveY = moveDir.y;

            animator.SetFloat("playerSpeed", Mathf.Abs(moveX));

            if (moveX > 0 && !isFacingRight)
            {
                //spriteRenderer.flipX = false;
                //transform.localScale = new Vector3(1, 1, 1);
                //swordCollider.offset = new Vector2(swordCollider.offset.x, swordCollider.offset.y);
                //playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y);
                flipPlayer(true);
                AudioController.audioInstance.playClip("walk");
            }
            else if (moveX < 0 && isFacingRight)
            {
                flipPlayer(false);
                AudioController.audioInstance.playClip("walk");

            }
        }

    }
    void flipPlayer(bool FacingRight)
    {
        isFacingRight = FacingRight;
        spriteRenderer.flipX = !isFacingRight;
        //transform.localScale = new Vector3(-1, 1, 1);
        swordCollider.offset = new Vector2(-swordCollider.offset.x, swordCollider.offset.y);
        playerCollider.offset = new Vector2(-playerCollider.offset.x, playerCollider.offset.y);
    }

    void FixedUpdate()
    {
        //Movement Vector Application
        rb.linearVelocity = new Vector2(moveX * playerSpeed * GameController.gcInstance.gameTime,
                                        rb.linearVelocityY * GameController.gcInstance.gameTime);

        //GroundCheck & Jump Logic
        CheckGround();
        if (grounded && rb.linearVelocityY < 0.01f)
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

    void onPausePerformed(InputAction.CallbackContext context)
    {
        if (GameController.gcInstance.isPaused)
        {
            uiController.resume();
        }
        else
        {
            uiController.pause();
        }
    }

    //Coroutine called since each needs a diff return type, needed to handle collider logic
    void onAttackPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("attackTrigger");
        AudioController.audioInstance.playClip("sword");
        swordCollider.enabled = true;
        yield return new WaitForSecondsRealtime(0.417f);
        swordCollider.enabled = false;
    }

    void onSavePerformed(InputAction.CallbackContext context)
    {
        if (canSave)
        {
            Debug.Log("Saved!");
            AudioController.audioInstance.playClip("save");
            GameController.gcInstance.playerPosition = gameObject.transform.position;
        }
        else
        {
            Debug.Log("Failed to save!");
        }
    }


    //To handle collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MoveText"))
        {
            uiController.SetActive(false, "MoveText");
            uiController.SetActive(true, "JumpText");
        }
        if (collision.CompareTag("JumpText"))
        {
            uiController.SetActive(false, "JumpText");
        }
        if (collision.CompareTag("Spike"))
        {
            //Player hit spike
            PlayerDamaged();
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            //maybe add blinking if damaged
        }
        if (collision.CompareTag("Save"))
        {
            canSave = true;
        }
        if (collision.CompareTag("Sword"))
        {
            //Player hit by boss
            PlayerDamaged();
        }
        if (collision.CompareTag("Enemy"))
        {
            //Player ran into enemy
            PlayerDamaged();
        }
        if (collision.CompareTag("BossTrigger"))
        {
            boss.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Save"))
        {
            canSave = false;
        }
    }

    void PlayerDamaged()
    {
        playerHealth--;
        AudioController.audioInstance.playClip("damage");
        if(playerHealth <= 0)
        {
            StartCoroutine(PlayerDeath());
        }
        StartCoroutine(Flash());
        Debug.Log("Enterdamage");

    }

    IEnumerator PlayerDeath()
    {
        isDead = true;
        animator.SetTrigger("death");
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject);
        GameController.gcInstance.LoadScene("MainMenu");
    }
    IEnumerator Flash()
    {
        int i = 2;
        for(int j = 0; j < i; ++j)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSecondsRealtime(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSecondsRealtime(0.1f);
            Debug.Log("flash");
        }
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

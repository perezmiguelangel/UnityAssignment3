using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BoxCollider2D bossCollider;
    public BoxCollider2D swordCollider;

    public int bossHealth = 10;
    public float bossSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float amplitude = 0.5f;  
    public float freq = 1f;
    public PlayerController player;
    public Color flashColor = Color.red;
    public Color originalColor;
    private Vector2 originalBossOffset;
    private Vector2 originalSwordOffset;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private float baseY;
    private float moveX;
    public float chaseRange = 6f;
    public float stopDist = 2f;

    void Awake()
    {
        originalColor = spriteRenderer.color;
        originalBossOffset = bossCollider.offset;
        originalSwordOffset = swordCollider.offset;
        baseY = transform.position.y;

        StartCoroutine(BossBehavior());
    }

    void FixedUpdate()
    {
        if (player == null || bossHealth <= 0) return;

        
        Vector2 currentPos = rb.position;
        float targetX = player.transform.position.x;

        if(Mathf.Abs(targetX - currentPos.x) > stopDist)
        {
            float stepX = bossSpeed * Time.fixedDeltaTime;
            float newX = Mathf.MoveTowards(currentPos.x, targetX, stepX);
            currentPos.x = newX;
        }
        

        // Sine wave for vertical pos
        float newY = baseY + Mathf.Sin(Time.time * freq) * amplitude;

        rb.MovePosition(new Vector2(currentPos.x, newY));
        moveX = currentPos.x - rb.position.x;
    }

    void Update()
    {
        if (moveX > 0 && !isFacingRight)
            Flip(true);
        else if (moveX < 0 && isFacingRight)
            Flip(false);
    }

    void Flip(bool faceRight)
    {
        isFacingRight = faceRight;
        spriteRenderer.flipX = !isFacingRight;

        bossCollider.offset = new Vector2(-bossCollider.offset.x, bossCollider.offset.y);
        swordCollider.offset = new Vector2(-swordCollider.offset.x, swordCollider.offset.y);
    }

    IEnumerator BossBehavior()
    {
        while (bossHealth > 0)
        {
            while (Vector2.Distance(transform.position, player.transform.position) > attackRange &&
                   Vector2.Distance(transform.position, player.transform.position) < chaseRange)
            {
                yield return null;
            }

            // Attack phase
            yield return StartCoroutine(Attack());

            // Small pause after attack
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Attack()
    {
        if (isAttacking) yield break;
        isAttacking = true;

        rb.linearVelocity = Vector2.zero; 
        animator.SetTrigger("attackTrigger");

        // Enable sword collider 
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        swordCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        swordCollider.enabled = false;

        isAttacking = false;

        // Cooldown before next attack
        yield return new WaitForSeconds(attackCooldown);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSword"))
        {
            Debug.Log("PlayerDamagedBoss!");
            BossDamaged();
        }
        if (collision.CompareTag("BossTriggerDeath"))
        {
            StartCoroutine(BossDeath());
        }
    }

    void BossDamaged()
    {
        bossHealth--;
        if (bossHealth <= 0)
        {
            StartCoroutine(BossDeath());
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BossDeath()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isDead", true);


        yield return new WaitForSeconds(2f); // death animation length
        bossCollider.enabled = false;
        swordCollider.enabled = false;
        Destroy(gameObject);
    }
}

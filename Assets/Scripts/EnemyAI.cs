using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float chaseDistance = 5f;
    public float attackDistance = 1.5f;

    public int damage = 1;
    public float attackCooldown = 1.5f;
    private float attackTimer;

    public Transform[] patrolPoints;
    public float waitTime = 2f;

    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Transform currentTarget;
    private float waitCounter;
    private bool isWaiting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>(); 
        }

        if (patrolPoints.Length > 0)
        {
            currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (playerHealth != null && playerHealth.isDead)
        {
            animator.SetBool("isRunning", false);
            return; 
        }

        bool isMoving = false;
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distToPlayer < attackDistance)
        {
            movement = Vector2.zero;
            isMoving = false;

            if (attackTimer <= 0)
            {
                animator.SetTrigger("Attack");
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                attackTimer = attackCooldown;
            }
        }
        else if (distToPlayer < chaseDistance)
        {
            currentTarget = playerTransform;
            MoveToTarget();
            isMoving = true;
            isWaiting = false;
        }
        else
        {
            if (patrolPoints.Length > 0)
            {
                if (Vector2.Distance(transform.position, currentTarget.position) < 0.2f)
                {
                    movement = Vector2.zero;
                    isMoving = false;
                    
                    if (!isWaiting)
                    {
                        isWaiting = true;
                        waitCounter = waitTime;
                    }
                    else
                    {
                        waitCounter -= Time.deltaTime;
                        if (waitCounter <= 0)
                        {
                            isWaiting = false;
                            currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
                        }
                    }
                }
                else
                {
                    MoveToTarget();
                    isMoving = true;
                }
            }
        }

        animator.SetBool("isRunning", isMoving);
    }

    void MoveToTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = currentTarget.position - transform.position;
        direction.Normalize();
        movement = direction;

        if (movement.x > 0) spriteRenderer.flipX = false;
        else if (movement.x < 0) spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
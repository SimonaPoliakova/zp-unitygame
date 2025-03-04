using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float gravity = 20f;
    public float jumpForceMin = 5f;
    public float jumpForceMax = 12f;
    public float detectionRange = 5f;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;  

    private Rigidbody2D rb;
    private Transform player;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        ScheduleNextJump();
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(facingRight ? 0.7f : -0.7f, 0.7f, 1);

        if (!IsGrounded())
        {
            rb.velocity += new Vector2(0, -gravity * Time.fixedDeltaTime);
        }

        if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    private void Patrol()
    {
        rb.velocity = new Vector2(facingRight ? speed : -speed, rb.velocity.y);

        if (IsTouchingWall())  
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 0.7f : -0.7f, 0.7f, 1);
        rb.velocity = new Vector2(facingRight ? speed : -speed, rb.velocity.y);
    }

    private void RandomJump()
    {
        if (IsGrounded())
        {
            float randomJump = Random.Range(jumpForceMin, jumpForceMax);
            rb.velocity = new Vector2(rb.velocity.x, randomJump);
        }
        ScheduleNextJump();
    }

    private void ScheduleNextJump()
    {
        float randomDelay = Random.Range(2f, 5f);
        Invoke("RandomJump", randomDelay);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);  
    }
}

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float gravity = 20f;
    public float jumpForceMin = 5f;
    public float jumpForceMax = 12f;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (IsTouchingWall())
        {
            Flip();
        }
    }

    private void Flip()
    {
        speed = -speed;
        facingRight = !facingRight;

        transform.localScale = new Vector3(facingRight ? 0.7f : -0.7f, 0.7f, 1);
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
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, groundLayer);
    }
}

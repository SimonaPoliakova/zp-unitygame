using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public GameObject bubblePrefab;  // Bubble prefab reference
    public float bubbleSpeed = 5f;   // Speed of the bubble

    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (move != 0)
        {
            spriteRenderer.flipX = move < 0;
        }

        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("run", move != 0 && isGrounded);

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("jump");
        }

        if (!wasGrounded && isGrounded)
        {
            animator.ResetTrigger("jump");
        }

        // Shoot bubble when pressing X
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShootBubble();
        }
    }

    private void ShootBubble()
    {
        animator.SetTrigger("blow");

        GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();

        if (bubbleRb != null)
        {
            float direction = spriteRenderer.flipX ? -1 : 1;
            bubbleRb.velocity = new Vector2(bubbleSpeed * direction, 0);
        }
    }
}

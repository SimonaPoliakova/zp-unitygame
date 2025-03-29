using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public GameObject bubblePrefab;

    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource jumpSound;
    private AudioSource bubbleSound;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        jumpSound = transform.Find("JumpSound")?.GetComponent<AudioSource>();
        bubbleSound = transform.Find("BubbleSound")?.GetComponent<AudioSource>();
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
            if (jumpSound != null)
            {

                jumpSound.Play();
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("jump");
        }

        if (!wasGrounded && isGrounded)
        {
            animator.ResetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ShootBubble();
        }
    }

    private void ShootBubble()
{
    if (bubbleSound != null)
    {
        bubbleSound.Play();
    }

    float direction = spriteRenderer.flipX ? -1 : 1;
    Vector3 spawnOffset = new Vector3(direction * 0.4f, -0.3f, 0); 
    GameObject bubble = Instantiate(bubblePrefab, transform.position + spawnOffset, Quaternion.identity);

    Bubble bubbleScript = bubble.GetComponent<Bubble>();
    if (bubbleScript != null)
    {
        bubbleScript.SetDirection(direction);
    }
}

}

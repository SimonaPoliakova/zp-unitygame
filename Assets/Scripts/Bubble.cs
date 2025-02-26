using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Sprite trappedSprite;
    public float floatSpeed = 2f;
    public float bobbingAmount = 0.2f;
    public float bobbingSpeed = 2f;
    public float speed = 3f;
    public float maxDistance = 2f;

    private SpriteRenderer spriteRenderer;
    private bool hasCapturedEnemy = false;
    private bool isFloating = false;
    private Transform bubbleStopPoint;
    private Vector3 stopPosition;
    private bool hasStopped = false;
    private float bobbingOffset;
    private Vector3 startPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        GameObject stopObject = GameObject.Find("BubbleStop");
        if (stopObject != null)
        {
            bubbleStopPoint = stopObject.transform;
        }

        bobbingOffset = Random.Range(0f, Mathf.PI * 2);
    }

    private void Update()
    {
        if (!hasCapturedEnemy && Vector3.Distance(startPosition, transform.position) < maxDistance)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if (!hasCapturedEnemy)
        {
            Destroy(gameObject);
        }

        if (isFloating && bubbleStopPoint != null)
        {
            if (transform.position.y < bubbleStopPoint.position.y)
            {
                transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            }
            else if (!hasStopped)
            {
                hasStopped = true;
                stopPosition = transform.position;
            }
        }

        if (hasStopped)
        {
            float bobbingY = Mathf.Sin(Time.time * bobbingSpeed + bobbingOffset) * bobbingAmount;
            transform.position = stopPosition + new Vector3(0, bobbingY, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasCapturedEnemy)
        {
            hasCapturedEnemy = true;

            if (trappedSprite != null)
            {
                spriteRenderer.sprite = trappedSprite;
            }

            Destroy(collision.gameObject);

            isFloating = true;

            Destroy(GetComponent<Rigidbody2D>());
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

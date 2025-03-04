using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject trappedBubblePrefab;
    public float floatSpeed = 2f;
    public float speed = 3f;
    public float maxDistance = 2f;

    private bool hasCapturedEnemy = false;
    private Vector3 startPosition;
    private float moveDirection = 1;

    public void SetDirection(float direction)
    {
        moveDirection = direction;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!hasCapturedEnemy && Vector3.Distance(startPosition, transform.position) < maxDistance)
        {
            transform.position += Vector3.right * moveDirection * speed * Time.deltaTime;
        }
        else if (!hasCapturedEnemy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasCapturedEnemy)
        {
            hasCapturedEnemy = true;
            CaptureEnemy(collision.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void CaptureEnemy(GameObject enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Destroy(enemy);

        if (trappedBubblePrefab != null)
        {
            Instantiate(trappedBubblePrefab, enemyPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

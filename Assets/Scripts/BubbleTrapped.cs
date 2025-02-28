using UnityEngine;

public class BubbleTrapped : MonoBehaviour
{
    public GameObject[] bonuses; 

    private Transform[] platforms;
    private Transform bubbleStopPoint;
    private bool hasStopped = false;
    private Vector3 stopPosition;
    private float floatSpeed = 2f;
    private float bobbingAmount = 0.2f;
    private float bobbingSpeed = 2f;
    private float bobbingOffset;

    private void Start()
    {
        GameObject stopObject = GameObject.Find("BubbleStop");
        if (stopObject != null)
        {
            bubbleStopPoint = stopObject.transform;
        }

        GameObject[] platformObjects = GameObject.FindGameObjectsWithTag("PlatformSpawn");
        platforms = new Transform[platformObjects.Length];

        for (int i = 0; i < platformObjects.Length; i++)
        {
            platforms[i] = platformObjects[i].transform;
        }

        bobbingOffset = Random.Range(0f, Mathf.PI * 2);
    }

    private void Update()
    {
        if (bubbleStopPoint != null)
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
        if (collision.CompareTag("Player"))
        {
            ReleaseBonus();
            Destroy(gameObject);
        }
    }

 private void ReleaseBonus()
{
    if (bonuses.Length == 0 || platforms.Length == 0)
    {
        Debug.LogWarning("No bonuses or platforms found!");
        return;
    }

    Transform selectedPlatform = platforms[Random.Range(0, platforms.Length)];

    float randomX = Random.Range(-8.0f, 8.0f); 

    Vector3 spawnPosition = new Vector3(
        selectedPlatform.position.x + randomX, 
        selectedPlatform.position.y, 
        selectedPlatform.position.z
    );

    GameObject randomBonus = bonuses[Random.Range(0, bonuses.Length)];

    Instantiate(randomBonus, spawnPosition, Quaternion.identity);
}


}

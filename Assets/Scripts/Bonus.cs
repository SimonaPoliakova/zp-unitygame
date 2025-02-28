using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int scoreValue = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(scoreValue); 
            Destroy(gameObject);
        }
    }
}

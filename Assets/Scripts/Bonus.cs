using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int bonusValue = 1000; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(bonusValue); 
            Destroy(gameObject); 
        }
    }
}

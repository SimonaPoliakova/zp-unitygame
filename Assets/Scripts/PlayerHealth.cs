using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    private TextMeshProUGUI healthText;
    private Vector3 startPosition; 

    private void Start()
    {
        startPosition = transform.position; 

        GameObject healthObj = GameObject.Find("HealthText");
        if (healthObj != null)
        {
            healthText = healthObj.GetComponent<TextMeshProUGUI>();
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("HealthText UI not found");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Respawn();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    private void Respawn()
    {
        transform.position = startPosition;
        Debug.Log("Player respawned");
    }

    private void Die()
    {
        Debug.Log("Player died");
        GameManager.Instance.ShowGameOver(); 
    }
}

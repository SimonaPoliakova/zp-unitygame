using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private int health = 2; 
    private TextMeshProUGUI healthText;

    private void Start()
    {
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
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "" + health;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");

    }
}

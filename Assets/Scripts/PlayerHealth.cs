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
            Die();
        else
            Respawn();
    }

   private void UpdateHealthUI()
{
    if (healthText == null)
    {
        GameObject healthObj = GameObject.Find("HealthText");
        if (healthObj != null)
        {
            healthText = healthObj.GetComponent<TextMeshProUGUI>();
        }
    }

    if (healthText != null)
    {
        healthText.text = health.ToString();
    }
}


    private void Respawn()
    {
        transform.position = startPosition;
    }

    private void Die()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = FindObjectOfType<GameManager>();

            if (GameManager.Instance == null)
                return;
        }

        GameManager.Instance.ShowGameOver();
    }
public void ResetHealth()
{
    health = 3;
    UpdateHealthUI();
}


}

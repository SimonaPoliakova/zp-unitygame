using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int bonusValue = 1000;
    private AudioSource bonusSound;

    private void Awake()
    {
        bonusSound = transform.Find("BonusSound")?.GetComponent<AudioSource>();
    }
   private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        if (bonusSound != null)
        {
            bonusSound.Play(); 
        }

        GameManager.Instance.AddScore(bonusValue);

        Destroy(gameObject, bonusSound.clip.length); 
    }
}

}

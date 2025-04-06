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
            bonusSound.transform.parent = null; // Detach from the object
            bonusSound.Play();
            Destroy(bonusSound.gameObject, bonusSound.clip.length); // Destroy sound after playback
        }

        GameManager.Instance.AddScore(bonusValue);
        Destroy(gameObject); // Instantly remove bonus object
    }
}

}

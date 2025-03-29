using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Vector3 newPosition = teleportTarget.position;
            newPosition.y += 1f;
            other.transform.position = newPosition;
        }
    }
}

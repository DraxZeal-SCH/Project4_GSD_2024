// This script represents a health drop that restores health to the player when picked up.
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    // Amount of health to add
    public int healthAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.IncreaseHealth(healthAmount);
            }
            Destroy(gameObject);
        }
    }
}

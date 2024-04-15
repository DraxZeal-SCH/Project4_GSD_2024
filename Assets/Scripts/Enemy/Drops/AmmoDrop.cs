// This script represents an ammo drop that adds ammo to the player when picked up.
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    // Amount of ammo to add
    public int ammoAmount = 30;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AddAmmo(ammoAmount);
            }
            Destroy(gameObject);
        }
    }
}

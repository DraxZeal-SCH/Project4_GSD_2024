// This script manages the pickups in the game.
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // Array of available pickups
    public GameObject[] pickups;

    // Method to spawn a random pickup at a specified position
    public void SpawnRandomPickup(Vector2 position)
    {
        if (pickups.Length > 0)
        {
            int randomIndex = Random.Range(0, pickups.Length); // Generate a random index
            Instantiate(pickups[randomIndex], position, Quaternion.identity); // Instantiate the pickup at the specified position
        }
        else
        {
            Debug.LogWarning("No pickups assigned to PickupManager.");
        }
    }
}

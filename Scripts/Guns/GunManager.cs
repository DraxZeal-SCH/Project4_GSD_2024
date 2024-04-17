// This script manages the switching of guns for the player.
using UnityEngine;

public class GunManager : MonoBehaviour
{
    // Array of available guns
    public GameObject[] availableGuns;

    // Index of the current gun in the available guns array
    public int currentGunIndex = 0;

    // Reference to the current player gun
    private GameObject currentPlayerGun;

    void Start()
    {
        // Assign the first available gun to the player if there are any available guns
        if (availableGuns.Length > 0)
        {
            SwitchGun(currentGunIndex);
        }
    }

    void Update()
    {
    }

    // Method to switch the current gun with a specified offset
    public void SwitchGun(int offset)
    {
        // Calculate the new gun index with the specified offset
        int newIndex = Mathf.Clamp(currentGunIndex + offset, 0, availableGuns.Length - 1);

        // Check if the new index is different from the current index
        if (newIndex != currentGunIndex)
        {
            currentGunIndex = newIndex; // Update the current gun index
            Destroy(currentPlayerGun); // Destroy the current player gun if it exists

            // Instantiate the new player gun at the manager's position and parent it to the manager
            currentPlayerGun = Instantiate(availableGuns[currentGunIndex], transform.position, Quaternion.identity, transform);
        }
    }

    // Method to shoot the current gun in a specified direction
    public void ShootCurrentGun(Vector3 shooterPosition, Vector2 shootDirection)
    {
        if (currentPlayerGun != null)
        {
            // Call the Fire method of the current gun
            Gun currentGunScript = currentPlayerGun.GetComponent<Gun>();
            if (currentGunScript != null)
            {
                currentGunScript.Fire(shootDirection);
            }
        }
    }

    // Method to reload the current gun
    public void ReloadCurrentGun()
    {
        if (currentPlayerGun != null)
        {
            // Call the Reload method of the current gun
            Gun currentGunScript = currentPlayerGun.GetComponent<Gun>();
            if (currentGunScript != null)
            {
                currentGunScript.Reload();
            }
        }
    }

    // Method to add ammo to the current gun
    public void AddAmmo(int amount)
    {
        if (currentPlayerGun != null)
        {
            // Call the AddAmmo method of the current gun
            Gun currentGunScript = currentPlayerGun.GetComponent<Gun>();
            if (currentGunScript != null)
            {
                currentGunScript.AddAmmo(amount);
            }
        }
    }
}

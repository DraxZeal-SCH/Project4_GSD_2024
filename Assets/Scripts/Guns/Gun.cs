// This script represents a generic gun in the game.
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Name of the gun
    public string gunName;

    // Prefab of the projectile fired by the gun
    public GameObject projectilePrefab;

    // Rate of fire (in seconds)
    public float fireRate = 0.5f;

    // Speed of the projectile
    public float projectileSpeed = 10f;

    // Damage dealt by the projectile
    public int damage = 20;

    // Used to make sure the projectile doesn't spawn in the player
    public Vector2 projectileOffset;

    // Maximum ammo in stockpile
    public int maxAmmo = 50;

    // Maximum ammo the gun can handle
    public int currentMaxAmmo = 10;

    // Current ammo in the gun
    private int currentAmmo;

    // Getter and setter for current ammo
    public int CurrentAmmo
    {
        get { return currentAmmo; }
        set { currentAmmo = value; }
    }

    // Getter and setter for current max ammo
    public int CurrentMaxAmmo
    {
        get { return currentMaxAmmo; }
        set { currentMaxAmmo = value; }
    }

    // Getter for max ammo
    public int MaxAmmo
    {
        get { return maxAmmo; }
    }

    void Start()
    {
        currentAmmo = currentMaxAmmo; // Initialize current ammo with current max ammo
    }

    // Method to fire the gun
    public void Fire(Vector2 direction)
    {
        if (currentAmmo > 0)
        {
            // Calculate the position of the projectile with the offset
            Vector3 spawnPosition = transform.position + (Vector3)projectileOffset;

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            rbProjectile.velocity = direction * projectileSpeed;
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Damage = damage; // Set projectile damage
            }
            currentAmmo--; // Decrease current ammo
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }

    // Method to reload the gun
    public void Reload()
    {
        int ammoToReload = maxAmmo - currentMaxAmmo; // Calculate ammo needed to reload
        if (ammoToReload > 0)
        {
            currentMaxAmmo += ammoToReload; // Increase current max ammo
            currentAmmo = currentMaxAmmo; // Refill current ammo
        }
        else
        {
            Debug.Log("No need to reload.");
        }
    }

    // Method to add ammo to the gun
    public void AddAmmo(int amount)
    {
        maxAmmo += amount; // Increase current max ammo
    }
}

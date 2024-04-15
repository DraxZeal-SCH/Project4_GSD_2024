// This script represents a projectile in the game.
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Private field for damage
    private int damage = 20;

    // Tags that the projectile should collide with and despawn when hit
    public string[] tags;

    // Private field for despawn time
    private float despawnTime = 2f;

    // Public property for damage with getter and setter
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    // Public property for despawn time with getter and setter
    public float DespawnTime
    {
        get { return despawnTime; }
        set { despawnTime = value; }
    }

    private float despawnTimer; // Timer to track despawn time

    void Start()
    {
        // Start the despawn timer
        despawnTimer = despawnTime;
    }

    void Update()
    {
        // Update the despawn timer
        despawnTimer -= Time.deltaTime;

        // Check if the despawn timer has reached zero
        if (despawnTimer <= 0f)
        {
            // If the timer has reached zero, destroy the projectile GameObject
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If the projectile collides with an enemy, deal damage to the enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the projectile when it collides with an enemy
            Destroy(gameObject);
        }
        // Check if the projectile collides with an obstacle or wall
        foreach (string tag in tags)
        {
            if (other.CompareTag(tag))
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;
    public float despawnTime = 2f; // Time in seconds before the projectile despawns

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
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            // Destroy the projectile when it collides with an enemy
            Destroy(gameObject);
        }
    }
}

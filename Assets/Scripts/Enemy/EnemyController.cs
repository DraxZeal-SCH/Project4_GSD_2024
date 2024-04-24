// This script controls the behavior of enemies in the game.
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // The speed of the enemies in scene
    public float speed = 3f;

    // Amount of health the enemies have
    public int currentHealth = 10;

    // Chance of dropping a pickup
    public float dropChance = 0.5f;

    // Amount of damage the enemies do
    public int attackDamage = 20;

    // Radius for obstacle detection
    public float obstacleCheckRadius = 1f;

    // Tags for obstacles
    public string[] obstacleTags;

    private Transform playerTransform; // Reference to the players position in the game
    private GameObject player; // Reference to the player gameobject in scene
    private PlayerController playerScript; // Reference to the PlayerController script
    private bool isDead = false; // Reference to the players state in the game
    private PickupManager pickupManager; // Reference to the PickupManager script
    private EnemySpawner enemySpawnerScript; // Reference to the EnemySpawner script
    private Rigidbody2D rb; // Reference to the rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
        }

        // Get the PickupManager component in the scene
        pickupManager = FindObjectOfType<PickupManager>();

        // Find and assign the EnemySpawner script reference
        GameObject enemySpawnerObject = GameObject.FindGameObjectWithTag("EnemySpawner");
        if (enemySpawnerObject != null)
        {
            enemySpawnerScript = enemySpawnerObject.GetComponent<EnemySpawner>();
        }
        else
        {
            Debug.LogError("No EnemySpawner object found in the scene!");
        }
    }

    void Update()
    {
        if (!isDead)
        {
            MoveTowardsPlayerWithObstacleAvoidance(); // Move the enemy towards the player with obstacle avoidance
        }
    }

    // Method to handle enemy movement towards the player with obstacle avoidance
    void MoveTowardsPlayerWithObstacleAvoidance()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized; // Direction towards the player
        Vector2 movement = direction * speed * Time.deltaTime; // Calculate movement
        if (!IsObstacleInPath()) // Check if there are obstacles in the way
        {
            rb.MovePosition(rb.position + movement); // Move the enemy towards the player
        }
    }

    // Method to check if there are obstacles in the way of the enemy
    bool IsObstacleInPath()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, obstacleCheckRadius); // Get colliders in the detection radius
        foreach (var collider in colliders)
        {
            foreach (var tag in obstacleTags)
            {
                if (collider.CompareTag(tag)) // Check if the collider has an obstacle tag
                {
                    return true; // Return true if an obstacle is detected
                }
            }
        }
        return false; // Return false if no obstacles are detected
    }

    // Method to take damage by the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Decrease health by the damage amount
        if (currentHealth <= 0)
        {
            Die(); // If health is less than or equal to zero, call the Die method
        }
    }

    // Method to handle enemy death
    void Die()
    {
        isDead = true; // Set isDead flag to true
        Destroy(gameObject); // Destroy the enemy GameObject

        if (playerScript != null)
        {
            playerScript.GainExp();
            playerScript.IncreaseHealth(10); // Increase the player's health by 10
        }

        if (pickupManager != null)
        {
            // Drop a pickup with a chance specified by the dropChance variable
            if (Random.value <= dropChance)
            {
                pickupManager.SpawnRandomPickup(transform.position); // Spawn a pickup at the enemy's position
            }
        }

        if (enemySpawnerScript != null)
        {
            enemySpawnerScript.EnemyDied(); // Notify the enemy spawner that an enemy has died
        }
    }

    // Method to handle enemy attack
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage); // Deal damage to the player
            }
        }
    }
}

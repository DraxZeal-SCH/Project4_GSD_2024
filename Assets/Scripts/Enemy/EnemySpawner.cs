// This script manages spawning and despawning of enemies.
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Prefab of the enemy to spawn
    public GameObject enemyPrefab;

    // Maximum number of enemies to spawn
    public int maxEnemies = 5;

    // Inner radius of the spawn area
    public float spawnRadiusInner = 5f;

    // Outer radius of the spawn area
    public float spawnRadiusOuter = 10f;

    // Delay before respawning enemies
    public float respawnDelay = 5f;

    // Distance threshold for despawning enemies
    public float despawnDistance = 20f;

    // Current number of spawned enemies
    private int currentEnemies;

    // Timer to track respawn time
    private float respawnTimer;

    // Reference to the player's transform
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find and store the player's transform
        SpawnEnemies(); // Spawn enemies when the game starts
    }

    void Update()
    {
        if (currentEnemies < maxEnemies)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                SpawnEnemies(); // Respawn enemies if the count is below the maximum
            }
        }

        DespawnFarAwayEnemies(); // Despawn enemies that move too far away from the spawner
    }

    // Method to spawn enemies
    void SpawnEnemies()
    {
        respawnTimer = respawnDelay; // Reset respawn timer

        int enemiesToSpawn = maxEnemies - currentEnemies;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 randomPos = GetRandomPositionAroundPlayer(); // Get random position around the player
            Instantiate(enemyPrefab, randomPos, Quaternion.identity); // Spawn an enemy at the random position
            currentEnemies++; // Increment the count of spawned enemies
        }
    }

    // Method to get a random position around the player within the spawn area
    Vector2 GetRandomPositionAroundPlayer()
    {
        Vector2 playerPos = playerTransform.position;

        // Calculate a random position within the donut-shaped region
        float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle
        float randomDistance = Random.Range(spawnRadiusInner, spawnRadiusOuter); // Random distance within the inner and outer radii
        Vector2 randomPos = playerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * randomDistance; // Calculate random position

        return randomPos;
    }

    // Method to despawn enemies that move too far away from the spawner
    void DespawnFarAwayEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies
        foreach (var enemy in enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) > despawnDistance)
            {
                Destroy(enemy); // Destroy enemy if it's too far away
                currentEnemies--; // Decrement the count of spawned enemies
            }
        }
    }

    // Method to visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        // Draw a donut-shaped region to visualize the spawn area around the player
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusOuter); // Outer circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusInner); // Inner circle
    }

    // Method called when an enemy dies to update the count of spawned enemies
    public void EnemyDied()
    {
        currentEnemies--; // Decrement the count of spawned enemies
    }
}

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public int maxEnemies = 5; // Maximum number of enemies to spawn
    public float spawnRadius = 5f; // Radius within which enemies will be spawned
    public float respawnDelay = 5f; // Delay before respawning enemies

    private int currentEnemies; // Current number of spawned enemies
    private float respawnTimer; // Timer to track respawn time

    void Start()
    {
        // Spawn enemies when the game starts
        SpawnEnemies();
    }
    void Update()
    {
        // Check if it's time to respawn enemies
        if (currentEnemies < maxEnemies)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        // Reset respawn timer
        respawnTimer = respawnDelay;

        // Calculate the number of enemies to spawn
        int enemiesToSpawn = maxEnemies - currentEnemies;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Calculate a random position within the spawn area
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // Spawn an enemy at the random position
            Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            currentEnemies++;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the spawn area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void EnemyDead(){
        currentEnemies -= 1;
    }
}

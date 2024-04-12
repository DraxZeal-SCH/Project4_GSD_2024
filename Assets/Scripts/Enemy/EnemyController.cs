using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public int currentHealth = 10;
    public GameObject healthItemPrefab; // Prefab of the health item to spawn
    public float healthItemSpawnChance = 1f; // Chance of spawning a health item (0 to 1)
    public int attackDamage = 20;

    private GameObject enemySpawner;
    private EnemySpawner enemySpawnerScript;
    private Transform playerTransform;
    private GameObject player;
    private PlayerController playerScript;
    
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");

        if(enemySpawner != null){
            enemySpawnerScript = enemySpawner.GetComponent<EnemySpawner>();
        }
        if(player != null){
            playerScript = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (!isDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            // Reduce the enemy's health when hit by a projectile
            TakeDamage(other.GetComponent<Projectile>().damage);
        }
        else if(other.CompareTag("Player"))
        {
            playerScript.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce the enemy's health
        currentHealth -= damage;

        // Check if the enemy's health is zero or less
        if (currentHealth <= 0)
        {
            // If the enemy's health is zero or less, destroy the enemy object
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        // Check if the health item should spawn based on the spawn chance
        if (Random.value <= healthItemSpawnChance)
        {
            // Spawn the health item at the enemy's position
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
        enemySpawnerScript.EnemyDead();
        // Destroy the enemy object
        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int projectileDamage = 20;
    public int healthAdded = 20;

    private int currentHealth;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isDead)
        {
            // Player movement input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Player shooting input
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            // Move the player
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead)
        {
            if (other.CompareTag("HealthItem"))
            {
                IncreaseHealth(healthAdded);
                Destroy(other.gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Game Over");
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        rbProjectile.velocity = shootDirection * projectileSpeed;
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = projectileDamage;
        }
    }
}

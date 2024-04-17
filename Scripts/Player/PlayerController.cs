// This script controls the player's movement and interaction.
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Default speed of the player
    public float defaultSpeed = 5f;

    // Speed of the player when shifted
    public float boostedSpeed = 10f;

    // Maximum health of the player
    public int maxHealth = 100;

    // Reference to the GunManager script
    public GunManager gunManager;

    // Key for reloading
    public KeyCode reloadKey = KeyCode.R;

    // Key for switching to the previous gun
    public KeyCode switchGunBackwardKey = KeyCode.Q;

    // Key for switching to the next gun
    public KeyCode switchGunForwardKey = KeyCode.E;

    // Current speed of the player
    private float currentSpeed;

    // Current health of the player
    private int currentHealth;

    // Rigidbody component of the player
    private Rigidbody2D rb;

    // Player movement input
    private Vector2 movement;

    // Flag to indicate if the player is dead
    private bool isDead = false;

    // Getter and setter for current health
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    // Getter and Setter for max health
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value;}
    }

    // Getter and setter for isDead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // Player movement input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Player shooting input
            if (Input.GetMouseButtonDown(0))
            {
                // Shoot towards the mouse cursor position
                ShootTowardsMouse();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Shoot in the opposite direction of the player's movement
                ShootBehindPlayer();
            }

            // Shift to change speed
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                currentSpeed = boostedSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentSpeed = defaultSpeed;
            }

            // Reload
            if (Input.GetKeyDown(reloadKey))
            {
                Reload();
            }

            // Switch guns backward
            if (Input.GetKeyDown(switchGunBackwardKey))
            {
                SwitchGun(-1);
            }

            // Switch guns forward
            if (Input.GetKeyDown(switchGunForwardKey))
            {
                SwitchGun(1);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            // Move the player
            rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore velocity by setting it to zero
            rb.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    
    public void AddAmmo(int amount)
    {
        // Call the AddAmmo method of the current gun
        if (gunManager != null)
        {
            gunManager.AddAmmo(amount);
        }
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Game Over");
    }

    void ShootTowardsMouse()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-coordinate is 0 (assuming 2D game)

        // Calculate the direction towards the mouse cursor
        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        // Shoot in the calculated direction
        if (gunManager != null)
        {
            gunManager.ShootCurrentGun(transform.position, shootDirection);
        }
    }

    void ShootBehindPlayer()
    {
        // Shoot in the opposite direction of the player's movement
        Vector2 shootDirection = -movement.normalized;

        // Shoot in the calculated direction
        if (gunManager != null)
        {
            gunManager.ShootCurrentGun(transform.position, shootDirection);
        }
    }

    void Reload()
    {
        if (gunManager != null)
        {
            // Call the Reload method of the current gun
            gunManager.ReloadCurrentGun();
        }
    }

    void SwitchGun(int direction)
    {
        if (gunManager != null)
        {
            gunManager.SwitchGun(direction);
        }
    }
}

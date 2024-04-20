// This script controls the player's movement and interaction.
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Default speed of the player
    [SerializeField] private float Speed = 5f;

    // Speed of the player when sprinting
    [SerializeField] private float sprintSpeed = 10f;

    // Maximum health of the player
    [SerializeField] private int maxHealth = 100;

    // Reference to the GunManager script
    private GunManager gunManager;

    // Current health of the player
    private int currentHealth;

    // Rigidbody component of the player
    private Rigidbody2D rb;

    // Flag to indicate if the player is dead
    private bool isDead = false;

    // Player movement input
    private Vector2 movement;

    // the position of the mouse relative to the player.
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        gunManager = GetComponentInChildren<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // Player movement input
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;

            transform.localRotation = Quaternion.Euler(0, 0, angle);
            

            if(Input.GetMouseButtonDown(0))
            {
                gunManager.ShootCurrentGun();
            }

            // Reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            // Switch guns backward
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchGun(-1);
            }

            // Switch guns forward
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchGun(1);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            // player movement with sprinting 
            if (Input.GetKey(KeyCode.LeftShift))// holding down left shift makes you sprint
            {
                rb.velocity = movement.normalized * sprintSpeed;
            }
            else
            {
                rb.velocity = movement.normalized * Speed;
            }
            
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

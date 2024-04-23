// This script controls the player's movement and interaction.
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Default speed of the player
    [SerializeField] private float Speed = 5f;

    // Speed of the player when sprinting
    [SerializeField] private float sprintSpeed = 10f;

    // Maximum health of the player
    [SerializeField] private int maxHealth = 100;

    [SerializeField] Text healthText;

    // Reference to the GunManager script
    private GunManager gunManager;

    // Current health of the player
    private int currentHealth;

    private int score;

    // Rigidbody component of the player
    private Rigidbody2D rb;

    // Flag to indicate if the player is dead
    private bool isDead = false;

    // Player movement input
    private Vector2 movement;

    private int ExperienceCap = 10;

    // The position of the mouse relative to the player.
    private Vector2 mousePos;

    // The current gun type for the gun being used. 0 = Semi-Auto, 1 = Full-Auto, 2 = Burst-Fire.
    private int currentGunType;

    // The fire rate for the currently equipped gun.
    private float currentFireRate;

    // The time between shots.
    private float fireTimer;

    // The number of bullets fired when using a burst fire gun.
    // private int bulletsToFire;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        gunManager = GetComponent<GunManager>();
        currentGunType = gunManager.GetCurrentGunType();
        currentFireRate = gunManager.GetCurrentGunFireRate();
        score = 0;
        //bulletsToFire = gunManager.GetBurstRounds();
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

            if (currentGunType == 0)
            {
                if (Input.GetMouseButtonDown(0) && fireTimer <= 0f)
                {
                    gunManager.ShootCurrentGun();
                    fireTimer = currentFireRate;
                }
                else
                {
                    fireTimer -= Time.deltaTime;
                }
            }
            else if (currentGunType == 1)
            {
                if (Input.GetMouseButton(0) && fireTimer <= 0f)
                {
                    gunManager.ShootCurrentGun();
                    fireTimer = currentFireRate;
                }
                else
                {
                    fireTimer -= Time.deltaTime;
                }
            }
            /* Still working on burst fire.
             * else if (currentGunType == 2)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    for (int i = 1; i < bulletsToFire; i++)
                    {
                        if (fireTimer <= 0f)
                        {
                            gunManager.ShootCurrentGun();
                            fireTimer = currentFireRate;
                        }
                        else
                        {
                            fireTimer -= Time.deltaTime;
                        }
                    }
                }
            }*/

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
        UpdateHealthText();
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
        GameOverTransition();
    }

    public void GameOverTransition()
    {
        SceneManager.LoadScene("GameOver");
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
            currentGunType = gunManager.GetCurrentGunType();
            currentFireRate = gunManager.GetCurrentGunFireRate();
            //bulletsToFire = gunManager.GetBurstRounds();
        }
    }

    public void AddScore(int value)
    {

    }
     void UpdateHealthText()
    {
          int displayedHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Update health text with current health value
        if (healthText != null)
        {
            healthText.text = "Health: " + displayedHealth.ToString(); // Display health value as text
        }
    }
}

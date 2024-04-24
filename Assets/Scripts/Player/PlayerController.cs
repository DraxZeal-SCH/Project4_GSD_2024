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
    [SerializeField] private int maxHealth;

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

    private int experience = 0;

    // The position of the mouse relative to the player.
    private Vector2 mousePos;

    // The current gun type for the gun being used. 0 = Semi-Auto, 1 = Full-Auto
    private int currentGunType;

    // The fire rate for the currently equipped gun.
    private float currentFireRate;

    // The time between shots.
    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 100;
        currentHealth = maxHealth;
        gunManager = GetComponent<GunManager>();
        currentGunType = gunManager.GetCurrentGunType();
        currentFireRate = gunManager.GetCurrentGunFireRate();
        score = 0;
       
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
        levelUp();
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
            
        }
    }

    public void AddScore(int value)
    {

    }
     void UpdateHealthText()
    {
         string health = "Health: " + currentHealth.ToString() + "/" + maxHealth.ToString();
        // Update health text with current health value
        if (healthText != null)
        {

            healthText.text = health; 
        }
    }

    public void GainExp()
    {
        experience++;
        Debug.Log("exp gained" + experience);
    }

    public void levelUp()
    {
        if(experience == ExperienceCap)
        {
            int oldMaxHealth = maxHealth;
            Debug.Log("OldMaxHealth: " + maxHealth);
            maxHealth += (20 / 100 * maxHealth);
            Debug.Log("NewMaxHealth: " + maxHealth);
            ExperienceCap += (20 / 100 * ExperienceCap);
            experience = 0;
            if(currentHealth >= oldMaxHealth)
            {
                currentHealth = maxHealth;
            }
            Debug.Log("Level Up");
        }
    }
}

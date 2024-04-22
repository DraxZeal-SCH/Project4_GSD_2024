using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float speed = 10f;// Speed property of the bullet. controlls the speed at which the bullet moves.
    
    [SerializeField] private float lifeTime = 3f;// Life Time of the bullet. how long the bullet stays "alive" before despawning

    [SerializeField] private int damage = 10;// The damage the bullet does when colliding with an enemy.

    [SerializeField] private bool penetration = false;// Wether the bullet is penetrating or not. allows the bullet to shoot through and inflict damage on more enemies.

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;// The bullet travels in the up direction relative to the bullet. the bullets up direction is dependant on its rotation.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If the projectile collides with an enemy, deal damage to the enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the projectile when it collides with an enemy unless penetration is toggled
            if (!penetration)
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    // Setters for use in the gun script for setting the class fields of the bullet.
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    public void SetPenetration(bool penetration)
    {
        this.penetration = penetration;
    }
}

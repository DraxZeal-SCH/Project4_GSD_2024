using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun2 : MonoBehaviour
{
    // The prefab for the bullet
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private int bulletDamage = 10;

    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private float bulletLifeTime = 3f;

    [SerializeField] private bool bulletPenetration = false;

    // The point from which the bullet is fired from.
    [SerializeField] private Transform firingPoint;

    // The guns rate of fire.
    [Range(0.001f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    // Wether the gun has infinite ammo or not.
    [SerializeField] private bool infiniteAmmo = false;

    // The maximum ammo this gun can have in reserve.
    [SerializeField] private int maxReserveAmmo = 50;

    /* 
     * The maximum size of the gun's Magazine.
     * You should aim to have your maximum reserve size.
     * be a multiple of your magazine size. 
     * so if your reserve is 50 and your mag size is 10 you would have 5 mags worth of ammo in reserve.
     * Ex. 10ammo x 5mags = 50reserve
    */
    [SerializeField] private int magSize = 10;

    // The type of gun 0 = Semi-auto, 1 = Full-Auto, 2 = Burst-fire
    [SerializeField] private int gunType = 0;

    // This Variable is only relavent to Burst fire guns. Controls the number of bullets per burst.
    // [SerializeField] private int burstRounds = 1;

    // The current ammo in the reserve.
    private int reserveAmmo = 0;

    // The current ammo loaded into the gun.
    private int loadedAmmo = 0;
    private Text ammoText;

    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip reload;

    private AudioSource source;
    

    void Start()
    {
        loadedAmmo = magSize;
        reserveAmmo = maxReserveAmmo;
        firingPoint = GameObject.Find("FiringPoint").transform;
        bulletPrefab.GetComponent<Bullet>().SetDamage(bulletDamage);
        bulletPrefab.GetComponent<Bullet>().SetSpeed(bulletSpeed);
        bulletPrefab.GetComponent<Bullet>().SetLifeTime(bulletLifeTime);
        bulletPrefab.GetComponent<Bullet>().SetPenetration(bulletPenetration);
        UpdateAmmoUI();
        ammoText = FindObjectsOfType<Text>()[0];
        source = gameObject.GetComponent<AudioSource>();
    }
     void Update()
    {
          UpdateAmmoUI();
    }
     void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            // Set the text to display loaded and reserve ammo counts
            ammoText.text = "Ammo: " + loadedAmmo + "/" + magSize + " | " + reserveAmmo;
        }
        else
        {
            Debug.LogWarning("Ammo Text component not assigned in Gun2 script!");
        }
    }


    // A method for shooting the gun with logic to handle the gun being out of ammo or having infinite ammo.
    public void Shoot()
    {
        if(loadedAmmo <= 0 && !infiniteAmmo)
        {
            Debug.Log("Out of Ammo! Time to Reload!");
        }
        else if (infiniteAmmo)
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            source.clip = shoot;
            source.volume = 0.5f;
            source.Play();

        }
        else
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            source.clip = shoot;
            source.volume = 0.5f;
            source.Play();
            loadedAmmo--;
        }

    }

    //A method for reloading the gun that accounts for corner cases.
    public bool Reload()
    {
        if(reserveAmmo > 0)
        {
            if(loadedAmmo > 0)
            {
                int ammoNeeded = magSize - loadedAmmo;// the ammo needed to refill the Mag when its only partially empty.
                Debug.Log("ammoNeeded: " + ammoNeeded);
                if (reserveAmmo < ammoNeeded)// if there is not enough ammo in reserve to fill the magazine.
                {
                    Debug.Log("Reserve: " + reserveAmmo);
                    loadedAmmo += reserveAmmo;//fill the mag with the remaining reserve ammo.
                    reserveAmmo = 0;//set reserve ammo to zero.
                    Debug.Log("Reserve: " + reserveAmmo);
                }
                else
                {
                    Debug.Log("Reserve: " + reserveAmmo);
                    loadedAmmo += ammoNeeded;//fill out the mag to its maximum size using ammo from the reserve.
                    reserveAmmo -= ammoNeeded;//subtract the ammo used from the reserve ammo.
                    Debug.Log("Reserve: " + reserveAmmo);
                }
                source.clip = reload;
                source.volume = 1;
                source.Play();
                return true;
            }
            else
            {
                if(reserveAmmo < magSize)// if the ammo in reserve is not enough to refill an entire Magazine.
                {
                    Debug.Log("Reserve: " + reserveAmmo);
                    loadedAmmo += reserveAmmo;// put all the remaining reserve ammo into the magazine.
                    reserveAmmo = 0;// set reserve ammo to zero.
                    Debug.Log("Reserve: " + reserveAmmo);

                }
                else
                {
                    Debug.Log("Reserve: " + reserveAmmo);
                    loadedAmmo += magSize;// refill the magazine to its maximum load.
                    reserveAmmo -= magSize;// subtract the ammo used from the reserve.
                    Debug.Log("Reserve: " + reserveAmmo);
                }
                source.clip = reload;
                source.volume = 1;
                source.Play();
                return true;
            }
        }
        else
        {
            Debug.Log("You have no reserve ammo left you'll need to find an ammo pickup!");
            return false;
        }
    }

    // A method for adding ammo into the reserve from an ammo pickup.
    public void AddAmmo(int amount)
    {
        if(reserveAmmo == maxReserveAmmo)
        {
            reserveAmmo = maxReserveAmmo;
            Debug.Log("You cannot hold any more ammo!");
        }
        else
        {
            reserveAmmo += amount;
            Debug.Log(amount + " Ammo added to your reserve.");
        }
    }

    public int GetGunType()
    {
        return gunType;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    /*public int GetBurstRounds()
    {
        return burstRounds;
    }*/
}

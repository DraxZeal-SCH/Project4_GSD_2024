using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun2 : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform firingPoint;

    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private bool infiniteAmmo = false;

    [SerializeField] private int maxReserveAmmo = 50;

    [SerializeField] private int magSize = 10;

    private int reserveAmmo = 0;

    private int loadedAmmo = 0;

    void Start()
    {
        loadedAmmo = magSize;
        reserveAmmo = maxReserveAmmo;
        firingPoint = GameObject.Find("FiringPoint").transform;
    }

    public void Shoot()
    {
        if(loadedAmmo <= 0 && !infiniteAmmo)
        {
            Debug.Log("Out of Ammo! Time to Reload!");
        }
        else if (infiniteAmmo)
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        }
        else
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            loadedAmmo--;
        }

    }

    public bool Reload()
    {
        if(reserveAmmo > 0)
        {
            if(loadedAmmo > 0)
            {
                int ammoNeeded = magSize - loadedAmmo;// the ammo needed to refill the Mag when its only partially empty.
                if (reserveAmmo < ammoNeeded)// if there is not enough ammo in reserve to fill the magazine.
                {
                    loadedAmmo += reserveAmmo;//fill the mag with the remaining reserve ammo.
                    reserveAmmo = 0;//set reserve ammo to zero.
                }
                else
                {
                    loadedAmmo += ammoNeeded;//fill out the mag to its maximum size using ammo from the reserve.
                    reserveAmmo -= ammoNeeded;//subtract the ammo used from the reserve ammo.
                }
                return true;
            }
            else
            {
                if(reserveAmmo < magSize)// if the ammo in reserve is not enough to refill an entire Magazine.
                {
                    loadedAmmo += reserveAmmo;// put all the remaining reserve ammo into the magazine.
                    reserveAmmo = 0;// set reserve ammo to zero.

                }
                else
                {
                    loadedAmmo += magSize;// refill the magazine to its maximum load.
                    reserveAmmo -= magSize;// subtract the ammo used from the reserve.
                }
                return true;
            }
        }
        else
        {
            Debug.Log("You have no reserve ammo left you'll need to find an ammo pickup!");
            return false;
        }
    }

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
}

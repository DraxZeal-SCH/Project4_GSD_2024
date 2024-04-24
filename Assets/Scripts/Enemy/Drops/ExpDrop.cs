using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GainExp();
            }
            Destroy(gameObject);
        }
    }
}

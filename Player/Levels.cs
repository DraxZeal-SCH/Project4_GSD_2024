using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    [SerializeField]
    private int levelExp = 0;

    private bool nextLevel;
   


    void Start()
    {
     nextLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
     /*if(PickupManager.pickedUp == true)
        {
            levelExp++;
        }
        */
    }
}

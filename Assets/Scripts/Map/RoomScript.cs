using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private float roomSize;
    [SerializeField] GameObject room;
    [SerializeField] float minimumSpaceBetweenObstacles = 2f;
    public GameObject[] obstacles;
    
    void Start()
    {
        roomSize = this.transform.localScale.x;
        SpawnObstacles();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // If the player enters the room, spawn more rooms in nearby areas
        if (other.GetComponent<Collider2D>().CompareTag("Player"))
        {
            SpawnRoomsWhereNeeded();
        }
    }

    private void SpawnRoomsWhereNeeded()
    {
        // Check the 8 adjacent areas around this room and spawn new rooms in empty areas
        for (float x = -roomSize; x <= roomSize; x += roomSize)
        {
            for (float y = -roomSize; y <= roomSize; y += roomSize)
            {
                float xSpawn = this.transform.position.x + x;
                float ySpawn = this.transform.position.y + y;

                if (!(RoomIsAtPosition(xSpawn, ySpawn)))
                {
                    Instantiate (room, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
                }
            }
        }
    }

    private bool RoomIsAtPosition(float x, float y)
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(new Vector3(x, y, 0), 1f);

        foreach (Collider2D col in intersecting)
        {
            if (col.tag == "Room")
            {
                return true;
            }
        }

        return false;
    }

    private bool ObjectIsAtPosition(float x, float y)
    {
        Collider2D[] intersecting = Physics2D.OverlapCircleAll(new Vector3(x, y, 0), minimumSpaceBetweenObstacles);

        foreach (Collider2D col in intersecting)
        {
            if (col.tag != "Room")
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnObstacles()
    {
        // Define area in which objects can be spawned
        float spawnMinX = this.transform.position.x - (roomSize/2);
        float spawnMinY = this.transform.position.y - (roomSize/2);
        float spawnMaxX = this.transform.position.x + (roomSize/2);
        float spawnMaxY = this.transform.position.y + (roomSize/2);

        // Go through the list of objects that can be spawned and for each one, place a random number of them into the room
        foreach (GameObject obstacle in obstacles)
        {
            SpawnSettings settings = obstacle.GetComponent<SpawnSettings>();
            int numberOfObstaclesToSpawn;
            if (settings != null)
            {
                numberOfObstaclesToSpawn = Random.Range(settings.minimumAmountToSpawn, settings.maximumAmountToSpawn);
            }
            else
            {
                numberOfObstaclesToSpawn = Random.Range(0, 10);
            }

            // Attempt to spawn the current object until an empty space is successfully found (or 80 failed attempts)
            for (int i = 0; i < numberOfObstaclesToSpawn; i ++)
            {
                bool foundEmptySpace = false;
                int attempts = 0;

                while (foundEmptySpace == false && attempts < 80)
                {
                    Vector3 spawnLocation = new Vector3(Random.Range(spawnMinX, spawnMaxX), Random.Range(spawnMinY, spawnMaxY), 0);
                    if (!ObjectIsAtPosition(spawnLocation.x, spawnLocation.y))  // To stop objects from spawning on top of each other
                    {
                        Instantiate(obstacle, spawnLocation, Quaternion.identity);
                        foundEmptySpace = true;
                    }
                    attempts ++;
                }
            }
        }
    }
}

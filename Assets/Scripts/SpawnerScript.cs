using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // This is the interval between spawns.
    [SerializeField] private float _spawnInterval;
    
    // This is a reference to the prefab that will be spawned.
    [SerializeField] private GameObject plane;

    // This is an array of the spawnable points.
    private static Vector2[] spawnablePoints = new Vector2[] {new Vector2(0, 8), new Vector2(11, 0), new Vector2(-11, 0)};
    // This is an array of the airline prefixes.
    [SerializeField] private static string[] airlinePrefix = new string[4] {"AI", "BA", "EY", "SV"};
    
    void Start()
    {
        // This starts the coroutine that will spawn the planes.
        StartCoroutine(SpawnPlane());
    }

    // This is the coroutine that will spawn the planes.
    IEnumerator SpawnPlane()
    {
        while (true)
        {
            // This waits for the specified amount of time.
            yield return new WaitForSeconds(_spawnInterval);
            
            // This instantiates plane prefab while also setting this instance to a variable.
            GameObject instance =  Instantiate(plane, transform.position, Quaternion.Euler(0, 0, RotationControl()));
            instance.name = PlaneNamer();
            //Shifts the position for spawner for next spawning
            transform.position = MoveSpawner();
        }
    }
    
    // This method will move the spawner to a random point on the screen.
    private Vector2 MoveSpawner()
    {
        //Randomly choose a side of the screen to spawn the plane
        int choice = Random.Range(0, 3);
        //Debug.Log(choice);
        //Return the randomised spawn point
        return spawnablePoints[choice];
    }
    
    // This method makes sure it is facing the right way at instantiation.
    private float RotationControl()
    {
        //Get the spawn position
        Vector2 spawnPosition = transform.position;
        float rot;
        
        //If the spawner is on the top side of the screen
        if (spawnPosition.y == 8)
        {
            rot = Random.Range(135, 226);
        }
        //If the spawner is on the right side of the screen
        else if (spawnPosition.x == 11)
        {
            rot = Random.Range(45, 136);
        }
        //If the spawner is on the left side of the screen
        else
        {
            rot = Random.Range(-135, -46);
        }
        //Return the randomised rotation within the specified range
        return rot;
    }

    // This method will return the name of the plane.
    private string PlaneNamer()
    {
        //Local variable to store the name of the plane
        string newName;
        //Randomly choose an Airline prefix
        newName = airlinePrefix[Random.Range(0, 4)];
        //Add a random flight number to the end of the prefix
        newName += Random.Range(0, 999).ToString();
        //Return the name of the plane
        return newName;
    }
}

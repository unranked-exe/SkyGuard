using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerScript : MonoBehaviour
{
    //Creates 1 instance of the SpawnerScript.
    public static SpawnerScript instance;

    //Holds the round the player is on.
    public int roundNumber = 0;

    // This is the interval between spawns.
    [SerializeField] private float _spawnInterval;
    // This is the number of planes to spawn in a round.
    [SerializeField] private float _planesToSpawn = 5;
    // This is the number of planes that have been spawned in a round.
    [SerializeField] private float _planesSpawned = 0;

    // This is a reference to the user plane prefab that will be spawned.
    [SerializeField] private GameObject plane;
    // This is a reference to the enemy plane prefab that will be spawned.
    [SerializeField] private GameObject skynet;

    // This is an array of the spawnable points.
    private static Vector2[] spawnablePoints = new Vector2[] {new Vector2(0, 8), new Vector2(11, 0), new Vector2(-11, 0)};
    // This is an array of the airline prefixes.
    private static string[] airlinePrefix = new string[4] {"AI", "BA", "EY", "SV"};
    
    void Awake()
    {
        //If there is no instance of the SpawnerScript, set it to this.
        if (instance == null)
        {
            instance = this;
            //Set the round number to 1
            roundNumber = 1;
        }
        //If there is an instance of the SpawnerScript, destroy this.
        else
        {
            Destroy(gameObject);
        }
    }

    // This method will check if all planes have been spawned in a round.
    private void CheckEndOfRound()
    {
        //Checks if all planes have been spawned.
        if (_planesSpawned == _planesToSpawn)
        {
            //Set the game state to EndOfRound.
            GameManager.instance.UpdateGameState(GameState.EndOfRound);
            //Increments the round number.
            roundNumber++;
            //Reset the number of planes spawned for the next round.
            _planesSpawned = 0;
            //Increases the number of planes to spawn in next round.
            _planesToSpawn += 2;
        }
    }
    
    // This is the coroutine that will spawn the planes.
    IEnumerator SpawnPlane()
    {
        //Checks if the game is playing
        while (GameManager.instance.State == GameState.Playing)
        {
            
            // This waits for the specified amount of time.
            yield return new WaitForSeconds(_spawnInterval);
            
            //For every 7th plane spawned and roundNumber > 3, spawn an enemy plane.
            if ((_planesSpawned % 7 == 0) && (_planesSpawned != 0) && (roundNumber > 3))
            {
                //Calls a method to spawn an enemy plane.
                SpawnEnemy();
                //Increments the number of planes spawned.
                _planesSpawned++;
            }
            else
            {
                // This instantiates a user plane.
                Instantiate(plane, transform.position, Quaternion.Euler(0, 0, RotationControl()));
                //Shifts the position for spawner for next spawning.
                transform.position = MoveSpawner();
                //Increments the number of planes spawned.
                _planesSpawned++;
                //Checks all planes to be spawned in round have been spawned.
                CheckEndOfRound();
            }
        }
        //Stops the coroutine. Using stop method inside of coroutine does not work on this instance.
        yield break;
    }

    private void SpawnEnemy()
    {
        // This instantiates an enemy plane.
        Instantiate(skynet, transform.position, Quaternion.Euler(0, 0, RotationControl()));
    }

    // This method will move the spawner to a random point on the screen.
    private Vector2 MoveSpawner()
    {
        //Randomly choose a side of the screen to spawn the plane.
        int choice = Random.Range(0, 3);
        //Debug.Log(choice);
        //Return the randomised spawn point.
        return spawnablePoints[choice];
    }
    
    // This method makes sure it is facing the right way at instantiation.
    private float RotationControl()
    {
        //Get the spawn position.
        Vector2 spawnPosition = transform.position;
        float rot;
        
        //Checks if the spawner is on the top side of the screen.
        if (spawnPosition.y == 8)
        {
            rot = Random.Range(135, 226);
        }
        //Checks if the spawner is on the right side of the screen.
        else if (spawnPosition.x == 11)
        {
            rot = Random.Range(45, 136);
        }
        //Checks if the spawner is on the left side of the screen.
        else
        {
            rot = Random.Range(-135, -46);
        }
        //Return the randomised rotation within the specified range.
        return rot;
    }

    public void StartSpawning()
    {
        //Starts the coroutine that will spawn the planes.
        StartCoroutine(SpawnPlane());
    }
    
    // This method will return the name of the plane.
    public string PlaneNamer()
    {
        //Local variable to store the name of the plane.
        string newName;
        //Randomly chooses an Airline prefix.
        newName = airlinePrefix[Random.Range(0, 4)];
        //Add a random flight number to the end of the prefix.
        newName += Random.Range(0, 999).ToString();
        //Returns the name of the plane.
        return newName;
    }
}

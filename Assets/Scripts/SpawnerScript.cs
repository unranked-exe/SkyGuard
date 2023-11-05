using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // This is the interval between spawns.
    [SerializeField] private float _spawnInterval;
    
    // This is a reference to the prefab that will be spawned.
    [SerializeField] private GameObject plane;
    
    void Start()
    {
        StartCoroutine(SpawnPlane());
    }

    IEnumerator SpawnPlane()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            Instantiate(plane, transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    [SerializeField] float minTime = 1f; // Minimum time between spawns
    [SerializeField] float meanTime = 5f; // Mean time between spawns (used for exponential distribution)
    [SerializeField] ObjectPool[] objectPools; // Array of object pools for spawning spikes
    [SerializeField] float spawnHeight = 20f; // Height above the ground to spawn spikes
    [SerializeField] float minX = -10f; // Minimum X position for spawning spikes
    [SerializeField] float maxX = 10f; // Maximum X position for spawning spikes

    float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnSpike();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        // Generate the time until the next spawn using the exponential distribution formula
        float spawnDelay = GetExponentialRandom(meanTime);
        nextSpawnTime = Time.time + Mathf.Max(spawnDelay, minTime); // Ensure the time is at least minTime
    }

    void SpawnSpike()
    {
        // Choose a random object pool from the array
        int randomPoolIndex = Random.Range(0, objectPools.Length);
        ObjectPool selectedPool = objectPools[randomPoolIndex];

        // Calculate a random X position for the spike
        float randomXPosition = Random.Range(minX, maxX);

        // Fixed Y position (spawn height) for the spike
        float spawnYPosition = spawnHeight;

        // Optional: If your pool only takes a float (e.g., z-offset)
        float randomZOffset = Random.Range(-2f, 2f); // Example z-offset if necessary

        // Enable a spike from the pool with a Z offset (if the pool method takes only floats)
        GameObject spike = selectedPool.EnableObjectInPool(randomZOffset);

        if (spike != null)
        {
            // Now manually set the position of the spike using Vector3
            spike.transform.position = new Vector3(randomXPosition, spawnYPosition, spike.transform.position.z);

            // Optionally, add more adjustments or set other properties here if needed
        }
        else
        {
            Debug.LogWarning("No available spikes in pool to spawn.");
        }
    }

    // Method to get an exponentially distributed random time based on meanTime
    float GetExponentialRandom(float mean)
    {
        // Exponential distribution inverse transform sampling
        float randomValue = Random.value; // Random number between 0 and 1
        return -mean * Mathf.Log(1 - randomValue); // Generate exponentially distributed random time
    }
}

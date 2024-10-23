// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] float minTime = 1f; // Minimum time between spawns
    [SerializeField] float meanTime = 5f; // Mean time between spawns (used for exponential distribution)
    [SerializeField] ObjectPool[] objectPools; // Array of object pools for spawning vehicles

    float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnVehicle();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        // Generate the time until the next spawn using the exponential distribution formula
        float spawnDelay = GetExponentialRandom(meanTime);
        nextSpawnTime = Time.time + Mathf.Max(spawnDelay, minTime); // Ensure the time is at least minTime
    }

    void SpawnVehicle()
    {
        // Choose a random object pool from the array
        int randomPoolIndex = Random.Range(0, objectPools.Length);
        ObjectPool selectedPool = objectPools[randomPoolIndex];

        // Calculate a random z-offset for vehicle spacing (optional for realism)
        float randomZOffset = Random.Range(-2f, 2f);

        // Enable a vehicle from the pool and apply the z-offset for random spacing
        GameObject vehicle = selectedPool.EnableObjectInPool(randomZOffset);

        if (vehicle != null)
        {
            // Optionally, add more adjustments or set other properties here if needed
        }
        else
        {
            Debug.LogWarning("No available vehicles in pool to spawn.");
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



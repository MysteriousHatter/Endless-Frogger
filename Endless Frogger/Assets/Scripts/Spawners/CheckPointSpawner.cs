using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSpawner : MonoBehaviour
{
    [SerializeField] int maxCoinCount = 3;   // Maximum number of coins to spawn
    [SerializeField] GameObject checkPointPrefab;  // Array of coin prefabs to spawn
    [SerializeField] float tileWidth = 50f;     // Width of the tile
    [SerializeField] float tileDepth = 10f;     // Depth of the tile

    private static int tileCounter = 0;        // Static counter to keep track of the number of tiles generated
    List<GameObject> coins = new List<GameObject>(); // List to keep track of spawned coins

    // Create coins when the tile is activated from the pool
    void OnEnable()
    {
        tileCounter++;  // Increment the tile counter every time a tile is activated

        // Check if it's the 4th or 6th tile in sequence
        if (tileCounter % 10 == 0)
        {
            for (int i = 0; i < Random.Range(1, maxCoinCount + 1); i++)  // Random number of coins
            {
                coins.Add(CreateCheckPoint());
            }
        }
    }

    // Destroy the coins when the tile is returned to the pool
    void OnDisable()
    {
        foreach (GameObject coin in coins)
        {
            Destroy(coin);  // Destroy the coin game objects
        }
        coins.Clear();  // Clear the list of coins
    }

    // Randomly place coins on the tile
    GameObject CreateCheckPoint()
    {
        GameObject coin = Instantiate(checkPointPrefab, transform);  
        // Randomize the coin's position within the tile bounds
        float xPos = Random.Range(-tileWidth / 2, tileWidth / 2);
        float zPos = Random.Range(-tileDepth / 2, tileDepth / 2);

        // Set the coin's position relative to the tile's position
        coin.transform.position = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);
        return coin;  // Return the spawned coin
    }
}
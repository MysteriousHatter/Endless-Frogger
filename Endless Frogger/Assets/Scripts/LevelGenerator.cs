// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] ObjectPool[] objectPools;
    [SerializeField] int worldSize = 10;
    [SerializeField] float tileDepth = 10f;
    [SerializeField] TileDespawner tileDespawner;
    [SerializeField] private float additonalSpacing = 0.9f;
    public GameObject checkpointTile; // Track the tile with the checkpoint
    private int tilesSpawned = 0; // Tracks the total number of tiles generated
    private bool gameHasStarted { get; set; }
    Mover mover;
    Player player;

    // List to track spawned tiles and their positions for checkpoint purposes
    List<GameObject> spawnedTiles = new List<GameObject>();
    Vector3 lastCheckpointPosition; // Store last checkpoint tile's position

    void Awake()
    {
        mover = FindObjectOfType<Mover>();
        player = FindObjectOfType(typeof(Player)) as Player;
        gameHasStarted = true;
    }

    void Start()
    {
        //Sets up the initial tiles in the world.
        for (int i = 0; i < worldSize; i++)
        {
            GenerateTile(tileDepth * i);
        }
    }

    public void DisableTile(GameObject obj)
    {
        
        //Return the tile to the pool
        obj.SetActive(false);

        //Stop the tile moving
        mover.RemoveMovableObject(obj);

        //Create a new replacement tile for the far end of the level
        GenerateTile(tileDepth * worldSize + (tileDespawner.transform.position.z + (2f * tileDepth)));
    }

    void GenerateTile(float zOffset)
    {
        //Select a tile from a random pool
        int selectedPool;
        if(gameHasStarted) { selectedPool = 0; } 
        else { selectedPool = Random.Range(0, objectPools.Length); }
        Debug.Log("Generate a tile");
        GameObject poolObject = objectPools[selectedPool].EnableObjectInPool(tileDepth);
        if (poolObject.CompareTag("Obstacle"))
        {
            // Define the additional space to add between tiles
            float additionalSpacing = 1f; // Adjust this value as needed
            Debug.Log("Obstcale");
            // Modify the zOffset for the current tile by adding the extra spacing
            zOffset += additionalSpacing;

            // Position the current tile based on the new zOffset
            poolObject.transform.position = new Vector3(poolObject.transform.position.x, poolObject.transform.position.y, zOffset);

            // Adjust zOffset for future tiles if necessary
            zOffset += 4f; // Assuming tileLength is the standard distance between tiles
        }
        //If there was not available tile in the pool, naively select the first one you can find from another pool
        if (poolObject == null)
        {
            if (gameHasStarted) { poolObject = objectPools[selectedPool].EnableObjectInPool(tileDepth); }
            else
            {
                foreach (ObjectPool pool in objectPools)
                {
                    poolObject = pool.EnableObjectInPool();
                    if (poolObject.CompareTag("Obstacle"))
                    {
                        // Define the additional space to add between tiles
                        float additionalSpacing = 1f; // Adjust this value as needed

                        // Modify the zOffset for the current tile by adding the extra spacing
                        zOffset += additionalSpacing;

                        // Position the current tile based on the new zOffset
                        poolObject.transform.position = new Vector3(poolObject.transform.position.x, poolObject.transform.position.y, zOffset);

                        // Adjust zOffset for future tiles if necessary
                        zOffset += 4f; // Assuming tileLength is the standard distance between tiles
                    }
                    if (poolObject != null) { break; }
                }
            }
        }

        if (poolObject != null)
        {
            //TODO Change the center alignment of the tile (or don't - it's your call!)
            poolObject.transform.position = new Vector3(0f, transform.position.y, transform.position.z + zOffset);

            //Make the tile moveable
            mover.AddMovableObject(poolObject);
        }
        gameHasStarted = false;

    }




}

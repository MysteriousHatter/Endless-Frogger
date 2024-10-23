// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] ObjectPool[] objectPools;
    [SerializeField] int worldSize = 10;
    [SerializeField] float tileDepth = 10f;
    [SerializeField] TileDespawner tileDespawner;
    [SerializeField] private float additonalSpacing = 0.9f;
    public GameObject checkpointTile; // Track the tile with the checkpoint
    private int tilesSpawned = 0; // Tracks the total number of tiles generated
    Mover mover;
    Player player;

    // List to track spawned tiles and their positions for checkpoint purposes
    List<GameObject> spawnedTiles = new List<GameObject>();
    Vector3 lastCheckpointPosition; // Store last checkpoint tile's position

    void Awake()
    {
        mover = FindObjectOfType<Mover>();
        player = FindObjectOfType(typeof(Player)) as Player;
    }

    void Start()
    {
        GenerateFirstTile();
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
        int selectedPool = Random.Range(0, objectPools.Length);
        Debug.Log("Generate a tile");
        GameObject poolObject = objectPools[selectedPool].EnableObjectInPool(tileDepth);
        //If there was not available tile in the pool, naively select the first one you can find from another pool
        if (poolObject == null)
        {
            foreach (ObjectPool pool in objectPools)
            {
                poolObject = pool.EnableObjectInPool();
                if (poolObject != null) { break; }
            }
        }

        if (poolObject != null)
        {
            //TODO Change the center alignment of the tile (or don't - it's your call!)
            poolObject.transform.position = new Vector3(0f, transform.position.y, transform.position.z + zOffset);

            if(poolObject.CompareTag("Swing"))
            {
                //TODO if pool object is swing then make sure instanitate an obstcale tile next to the swing t
            }

            //Make the tile moveable
            mover.AddMovableObject(poolObject);
        }
    }

    void GenerateFirstTile()
    {
        // You can select a specific pool or a specific tile prefab for the first tile
        GameObject firstTile = objectPools[0].EnableObjectInPool(tileDepth); // Assuming pool[0] contains the starting tile

        if (firstTile != null)
        {
            // Set position of the first tile at the start of the game
            firstTile.transform.position = new Vector3(0f, transform.position.y, 0f);

            // Make the first tile moveable if needed
            mover.AddMovableObject(firstTile);
        }
        else
        {
            Debug.LogWarning("No available tile for the first tile in the pool.");
        }
    }



}

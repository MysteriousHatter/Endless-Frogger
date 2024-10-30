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
    // List to track spawned tiles and their positions for checkpoint purposes
    List<GameObject> spawnedTiles = new List<GameObject>();
    private Vector3 lastTilePosition;

    private bool gameHasStarted { get; set; }
    Mover mover;
    Player player;

    void Awake()
    {
        mover = FindObjectOfType<Mover>();
        player = FindObjectOfType(typeof(Player)) as Player;
        lastTilePosition = transform.position; // Initial position of the first tile
        gameHasStarted = true;
    }

    void Start()
    {
        //Sets up the initial tiles in the world.
        RegenerateTiles();
    }

    private void RegenerateTiles()
    {
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

        // Remove the tile from the spawnedTiles list
        spawnedTiles.Remove(obj);

        Debug.Log("Current spawned tile count " + spawnedTiles.Count);

        // Calculate the position for the new tile based on the last tile in spawnedTiles
        float zOffset = (spawnedTiles.Count > 0)
            ? spawnedTiles[spawnedTiles.Count - 1].transform.position.z + tileDepth
            : lastTilePosition.z + tileDepth;

        // Generate a new tile at the calculated position
        GenerateTile(zOffset);
    }

    void GenerateTile(float zOffset)
    {
        //Select a tile from a random pool
        int selectedPool;
        if (gameHasStarted) { selectedPool = 0; }
        else { selectedPool = Random.Range(0, objectPools.Length); }
        Debug.Log("Generate a tile");
        GameObject poolObject = objectPools[selectedPool].EnableObjectInPool(tileDepth);
        if (gameHasStarted) { RespawnPlayerPostion(poolObject.transform); }
        if (poolObject != null) // Null check to prevent NullReferenceException
        {
            if (poolObject.CompareTag("Obstacle"))
            {
                // Define the additional space to add between tiles
                float additionalSpacing = 1f; // Adjust this value as needed
                Debug.Log("Obstacle");

                // Modify the zOffset for the current tile by adding the extra spacing
                zOffset += additionalSpacing;

                // Position the current tile based on the new zOffset
                poolObject.transform.position = new Vector3(poolObject.transform.position.x, poolObject.transform.position.y, zOffset);

                // Adjust zOffset for future tiles if necessary
                zOffset += 4f; // Assuming tileLength is the standard distance between tiles
            }
        }
        else
        {
            Debug.LogWarning("No available object in pool for the selected tileDepth.");
        }

        //If there was not available tile in the pool, naively select the first one you can find from another pool
        if (poolObject == null)
        {
            if (gameHasStarted)
            {
                poolObject = objectPools[selectedPool].EnableObjectInPool(tileDepth);
                RespawnPlayerPostion(poolObject.transform);
            }
            else
            {
                foreach (ObjectPool pool in objectPools)
                {
                    poolObject = pool.EnableObjectInPool();
                    if (poolObject != null) // Null check to prevent NullReferenceException
                    {
                        if (poolObject.CompareTag("Obstacle"))
                        {
                            // Define the additional space to add between tiles
                            float additionalSpacing = 1f; // Adjust this value as needed
                            Debug.Log("Obstacle");

                            // Modify the zOffset for the current tile by adding the extra spacing
                            zOffset += additionalSpacing;

                            // Position the current tile based on the new zOffset
                            poolObject.transform.position = new Vector3(poolObject.transform.position.x, poolObject.transform.position.y, zOffset);

                            // Adjust zOffset for future tiles if necessary
                            zOffset += 4f; // Assuming tileLength is the standard distance between tiles
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No available object in pool for the selected tileDepth.");
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
            spawnedTiles.Add(poolObject);

        }
        gameHasStarted = false;

    }

    private void RespawnPlayerPostion(Transform tile)
    {
        // Get the tile's position
        Vector3 tilePosition = tile.transform.position;

        // Calculate the new player position above the tile
        Vector3 newPosition = new Vector3(0f, tilePosition.y + 1.5f, 0);

        //// Set the player's position
        player.transform.position = newPosition;
    }

    public void ReloadTiles()
    {
        if (GameManager.instance.getCoinCollection() >= 5)
        {
            // Clear current tiles and respawn them from the checkpoint
            foreach (var tile in spawnedTiles)
            {
                tile.SetActive(false); // Disable all current tiles
            }

            spawnedTiles.Clear(); // Clear the list
            gameHasStarted = true;
            GameManager.instance.GameOverUI.SetActive(false);
            RegenerateTiles();
            FindObjectOfType<Player>().enabled = true;
            FindAnyObjectByType<Mover>().enabled = true;
            GameManager.instance.SetCoinCollection(GameManager.instance.getCoinCollection() - 5);
        }
    }



}
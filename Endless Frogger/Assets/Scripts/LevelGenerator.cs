// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPool[] objectPools;
    [SerializeField] private int worldSize = 10;
    [SerializeField] private float tileDepth = 10f;
    [SerializeField] private TileDespawner tileDespawner;
    [SerializeField] private float additionalSpacing = 0.9f;

    private List<GameObject> spawnedTiles = new List<GameObject>();
    private GameObject checkpointTile;
    private bool gameHasStarted { get; set; }
    private Mover mover;
    private Player player;

    void Awake()
    {
        mover = FindObjectOfType<Mover>();
        player = FindObjectOfType<Player>();
        gameHasStarted = true;
    }

    void Start()
    {
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
        obj.SetActive(false);
        mover.RemoveMovableObject(obj);
        spawnedTiles.Remove(obj);

        GenerateTile(tileDepth * worldSize + (tileDespawner.transform.position.z + (2f * tileDepth)));
    }

    void GenerateTile(float zOffset)
    {
        GameObject poolObject = SelectRandomTile(zOffset);
        if (poolObject == null) return;

        poolObject.transform.position = new Vector3(0f, transform.position.y, transform.position.z + zOffset);
        mover.AddMovableObject(poolObject);
        spawnedTiles.Add(poolObject);

        gameHasStarted = false;
    }

    private GameObject SelectRandomTile(float zOffset)
    {
        int selectedPoolIndex = gameHasStarted ? 0 : Random.Range(0, objectPools.Length);
        GameObject poolObject = objectPools[selectedPoolIndex].EnableObjectInPool();

        if (poolObject == null)
        {
            // Fallback: Iterate through pools if no tile was available
            poolObject = FallbackObjectSelection();
            if (poolObject == null)
            {
                Debug.LogWarning("No available objects in any pool.");
                return null;
            }
        }

        ApplySpacing(poolObject, ref zOffset);
        if (gameHasStarted) RespawnPlayerPosition(poolObject.transform);

        return poolObject;
    }

    private GameObject FallbackObjectSelection()
    {
        foreach (ObjectPool pool in objectPools)
        {
            GameObject fallbackObject = pool.EnableObjectInPool();
            if (fallbackObject != null)
                return fallbackObject;
        }
        return null;
    }

    private void ApplySpacing(GameObject poolObject, ref float zOffset)
    {
        if (poolObject.CompareTag("Obstacle"))
        {
            zOffset += additionalSpacing + 1f;
        }
        else
        {
            zOffset += additionalSpacing;
        }
    }

    private void RespawnPlayerPosition(Transform tile)
    {
        Vector3 newPosition = new Vector3(0f, tile.position.y + 1.5f, 0f);
        player.transform.position = newPosition;
    }

    public void ReloadTiles()
    {
        if (GameManager.instance.getCoinCollection() >= 5)
        {
            foreach (var tile in spawnedTiles)
            {
                tile.SetActive(false);
            }

            spawnedTiles.Clear();
            gameHasStarted = true;
            GameManager.instance.GameOverUI.SetActive(false);
            RegenerateTiles();
            player.enabled = true;
            mover.enabled = true;
            GameManager.instance.SetCoinCollection(GameManager.instance.getCoinCollection() - 5);
        }
    }
}


// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] ObjectPool[] objectPools;
    [SerializeField] int worldSize = 10;
    [SerializeField] float tileDepth = 10f;
    [SerializeField] TileDespawner tileDespawner;

    Mover mover;

    void Awake()
    {
        mover = FindObjectOfType<Mover>();
    }

    void Start()
    {
        //Sets up the initial tiles in the world.
        for (int i = -1; i < worldSize; i++)
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

        if(poolObject != null)
        {
            //TODO Change the center alignment of the tile (or don't - it's your call!)
            poolObject.transform.position = new Vector3(0f, transform.position.y, transform.position.z + zOffset);

            //Make the tile moveable
            mover.AddMovableObject(poolObject);
        }
    }
}

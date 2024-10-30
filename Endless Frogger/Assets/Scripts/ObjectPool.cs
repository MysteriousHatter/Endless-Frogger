// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] int poolSize;

    GameObject[] pool;
    public string poolName;

    void Awake()
    {
        PopulatePool();
    }

    public GameObject EnableObjectInPool(float zOffset = 0f)
    {
        //Find the next available object in the pool and try to enable it
        for (int i = 0; i < pool.Length; i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zOffset);
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        return null;
    }


    public void DisableObjectInPool(GameObject obj)
    {
        Debug.Log("The obj name " +  obj.name);
        obj.SetActive(false);
    }


    void PopulatePool()
    {
        //Create the pool
        pool = new GameObject[poolSize];

        //Instantiate the objects into the pool and disable them
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(objectPrefab, transform);
            pool[i].SetActive(false);
        }
    }
    public GameObject EnableObjectWithTag(string tag, float zOffset = 0f)
    {
        foreach (GameObject obj in pool)
        {
            // Check if the object is inactive and matches the tag
            if (!obj.activeInHierarchy && obj.CompareTag(tag))
            {
                // Set the position based on the current object's position plus zOffset
                obj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zOffset);

                // Enable the object in the hierarchy
                obj.SetActive(true);
                return obj;
            }
        }

        // Return null if no matching object was found
        return null;
    }


    //Called by Unity
    void OnValidate()
    {
        //Prevent the designer from entering an invalid pool size
        if (poolSize < 0) { poolSize = 0; }
    }
}

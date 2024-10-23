// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] int maxObstacleCount = 5;
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] float tileWidth = 50f;
    [SerializeField] float tileDepth = 10f;

    List<GameObject> obstacles = new List<GameObject>();

    //TODO - Could these obstacles all be handled with an object pool instead of Instantiate/Destroy? 

    //Create set dressing (trees, rocks etc.) when the tile is activated from a pool
    void OnEnable()
    {
        for (int i = 0; i < Random.Range(0, maxObstacleCount); i++)
        {
             obstacles.Add(CreateObstacle());
        }
    }

    //Destroy the obstacles when the tile is returned to the pool
    void OnDisable()
    {
        foreach(GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }

    //Randomly place obstacles on the tile 
    GameObject CreateObstacle()
    {
        int obstaclePrefabIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(obstaclePrefabs[obstaclePrefabIndex], transform);

        float xPos = Random.Range(-tileWidth / 2, tileWidth / 2);
        float zPos = Random.Range(-tileDepth / 2, tileDepth / 2);

        obstacle.transform.position = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);
        return obstacle;
    }
}

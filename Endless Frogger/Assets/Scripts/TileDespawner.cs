// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileDespawner : MonoBehaviour
{
    LevelGenerator levelGenerator;
    const string objectTag = "Tile";
    [SerializeField] private float maxObstacleSpeed = 20f; // Maximum speed cap
    [SerializeField] private int speedIncrementPoints = 10; // Points threshold for each speed increase
    [SerializeField] private float speedIncreaseAmount = 4f; // Amount to increase per threshold

    void Awake()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    //Return a tile to the pool when it reaches the despawner
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == objectTag || other.tag == "Swing" || other.tag == "Obstacle")
        {
            if (levelGenerator != null)
            {
                ScoreManager.instance.TileCleared();
                levelGenerator.DisableTile(other.gameObject);
                AdjustDifficulty();
            }
        }
    }

    private void AdjustDifficulty()
    {
        // Check if coin collection has reached the next threshold for a speed increase
        int speedLevel = ScoreManager.instance.score / speedIncrementPoints;     
        float targetSpeed = Mathf.Min(speedLevel * speedIncreaseAmount, maxObstacleSpeed);
        Debug.Log("Increase the speed " + targetSpeed + "By the obstacle speed " + GameManager.instance.obstacleSpeed);

        if (GameManager.instance.obstacleSpeed <= targetSpeed)
        {
            GameManager.instance.obstacleSpeed = targetSpeed + 1;
            UpdateObstacleSpeed();
        }
    }

    private void UpdateObstacleSpeed()
    {
        // Get all active obstacle and vehicle scripts in the scene
       Spikes[] obstacles = FindObjectsOfType<Spikes>();
        Vehicle[] vehicles = FindObjectsOfType<Vehicle>();

        // Update the speed for each obstacle
        foreach (Spikes obstacle in obstacles)
        {
            obstacle.SetSpeed(GameManager.instance.obstacleSpeed);
        }

        // Update the speed for each vehicle
        foreach (Vehicle vehicle in vehicles)
        {
            vehicle.SetSpeed(GameManager.instance.obstacleSpeed);
        }
    }

}

// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;
using UnityEngine.SceneManagement;

public class TileDespawner : MonoBehaviour
{
    LevelGenerator levelGenerator;
    const string objectTag = "Tile";

    void Awake()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    //Return a tile to the pool when it reaches the despawner
    void OnTriggerEnter(Collider other) {
        if(other.tag == objectTag || other.tag == "Checkpoint" || other.tag == "Swing" || other.tag == "Obstacle")
        {
            if(levelGenerator != null)
            {
                ScoreManager.instance.TileCleared();
                levelGenerator.DisableTile(other.gameObject);
            }
        }
    }
}

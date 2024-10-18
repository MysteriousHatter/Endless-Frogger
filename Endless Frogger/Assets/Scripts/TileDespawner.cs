// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

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
        if(other.tag == objectTag)
        {
            if(levelGenerator != null)
            {
                levelGenerator.DisableTile(other.gameObject);
            }
        }
    }
}

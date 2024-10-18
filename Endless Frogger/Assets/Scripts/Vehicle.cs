// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float speed;

    const string despawnerTag = "Despawner";

    void OnEnable()
    {
        transform.rotation = transform.parent.rotation;
    }
        
    //TODO - Move the vechicle along the tile (note: the vehicle will already move in relation to the parent tile)
    //TODO - Handle the removal of the vehicle when it reaches the end of the tile
}

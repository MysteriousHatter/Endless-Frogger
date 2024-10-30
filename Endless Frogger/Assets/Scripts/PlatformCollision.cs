using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] Transform platform;
    [SerializeField] private Vehicle vehicle;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals(playerTag))
        {
            collision.gameObject.transform.parent = platform;
            Debug.Log("Get on log");
        }
        else if(collision.gameObject.tag.Equals("Despawner"))
        {
            Debug.Log("Despawn Log");
            vehicle.despanwerPool.DisableObjectInPool(this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals(playerTag))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}

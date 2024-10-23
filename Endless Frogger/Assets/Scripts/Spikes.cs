using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] float speed;

    const string despawnerTag = "Despawner"; // Tag used for despawning vehicles

    Rigidbody rb;

    public ObjectPool despanwerPool;

    void OnEnable()
    {
        transform.rotation = transform.parent.rotation; // Ensure vehicle aligns with the parent's rotation
        despanwerPool = FindObjectOfType<ObjectPool>();

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            // Add a Rigidbody if it doesn't exist
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Rigidbody was missing and has been added.");
        }

        // Make the Rigidbody kinematic since you are manually controlling the movement
        rb.isKinematic = true;
    }

    void Update()
    {
        // Move the vehicle forward along the tile based on its own speed
        MoveVehicle();
    }

    void MoveVehicle()
    {
        // Move the object downward (along the y-axis) with the specified speed
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // If the vehicle hits a despawner trigger, it should disable itself
        if (other.CompareTag(despawnerTag))
        {
            Debug.Log("Despawn Car");
            despanwerPool.DisableObjectInPool(this.gameObject);

        }
    }
}

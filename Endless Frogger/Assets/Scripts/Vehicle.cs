// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float speed;

    const string despawnerTag = "Despawner"; // Tag used for despawning vehicles

    Rigidbody rb;

    public ObjectPool despanwerPool;
    private Player player;

    void OnEnable()
    {
        transform.rotation = transform.parent.rotation; // Ensure vehicle aligns with the parent's rotation
        despanwerPool = FindObjectOfType<ObjectPool>();

        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType(typeof(Player)) as Player;

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

        // Check if the player is on the platform and presses the jump button
        if (player != null && player.JumpInput)
        {
            // Player has jumped, so unparent them from the platform (log)
            player.transform.SetParent(null);
            Debug.Log("Player has jumped off the platform.");
        }
    }

    void MoveVehicle()
    {
        // Move the vehicle along its forward direction (z-axis in most setups) with the specified speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            // Parent the player object to the vehicle (platform)
            collisionInfo.gameObject.transform.SetParent(this.transform);

            Debug.Log("Player is now parented to the platform");
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            // Unparent the player object from the platform when they leave
            collisionInfo.gameObject.transform.SetParent(null);

            Debug.Log("Player has left the platform and is unparented");
        }
    }

}

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
        // Now GameManager.instance should be ready
        if (GameManager.instance != null)
        {
            speed = GameManager.instance.getObstacleSpeed();
        }
        else
        {
            Debug.LogWarning("GameManager instance not found. Default speed will be used.");
            speed = 5f; // Default speed if GameManager isn't found (optional)
        }
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

    public float getSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

}

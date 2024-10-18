using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    private Transform player => FindObjectOfType<Player>().transform;               // Reference to the player's transform
    public float lockOnRange = 10f;        // Distance at which lock-on is allowed
    public float facingAngleThreshold = 45f; // Angle the player must be within to be "facing" the enemy
    public LayerMask enemyLayer;           // Layer to detect enemies
    public GameObject homingReticlePrefab; // Prefab of the homing reticle to display above enemies

    private GameObject currentReticle;     // Currently active homing reticle
    private Transform lockedEnemy;         // Currently locked-on enemy
 

    void Update()
    {
        // Check for nearby enemies
        Collider[] enemiesInRange = Physics.OverlapSphere(player.position, lockOnRange, enemyLayer);

        // Reset lock-on if no enemies are detected
        if (enemiesInRange.Length == 0)
        {
            Debug.Log("Are collider is empty");
            DisableReticle();
            lockedEnemy = null;
            return;
        }

        // Find the first valid enemy the player is facing
        foreach (Collider enemy in enemiesInRange)
        {
            Transform enemyTransform = enemy.transform;

            // Check if the player is facing the enemy
            if (IsPlayerFacingEnemy(player, enemyTransform))
            {
                // Set the locked enemy and display the homing reticle
                lockedEnemy = enemyTransform;
                player.GetComponent<Player>().currentEnemy = lockedEnemy.transform;
                ShowReticle(lockedEnemy);
                player.GetComponent<Player>().setIsLockedOn(true);
                return;
            }
        }

        // If no valid enemies, disable the reticle
        DisableReticle();
        lockedEnemy = null;
        player.GetComponent<Player>().currentEnemy = null;
    }

    // Method to check if the player is facing the enemy
    bool IsPlayerFacingEnemy(Transform player, Transform enemy)
    {
        Vector3 directionToEnemy = (enemy.position - player.position).normalized;

        // Calculate the angle between the player's forward direction and the direction to the enemy
        float angleToEnemy = Vector3.Angle(player.forward, directionToEnemy);

        // Return true if the player is within the facing angle threshold
        return angleToEnemy < facingAngleThreshold;
    }

    // Method to display the homing reticle above the enemy
    void ShowReticle(Transform enemy)
    {
        if (currentReticle == null)
        {
            // Instantiate the homing reticle if it doesn't already exist
            currentReticle = Instantiate(homingReticlePrefab, this.transform);
            currentReticle.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position);

        }
        else
        {
            // Update the position of the reticle above the locked enemy
            currentReticle.transform.position = Camera.main.WorldToScreenPoint(enemy.position);
        }
    }

    // Method to disable the homing reticle
    void DisableReticle()
    {
        if (currentReticle != null || player.GetComponent<Player>().StateMachine.CurrentState == player.GetComponent<Player>().SwingState)
        {
            player.GetComponent<Player>().setIsLockedOn(false);
            Destroy(currentReticle);
        }
    }

    // Visualize the lock-on range in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, lockOnRange);
    }
}

// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerStompState StompState { get; private set; }
    public PlayerSwingState SwingState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public GameObject enemyPrefab { get; set; }

    public bool playerDead { get; set; }

    public event Action HitKid;

    public Transform GrappleDirectionIndicator { get; private set; }

    public SphereCollider MovementCollider { get; private set; }

    public PlayerSwingState GrappleDirectionalState { get; private set; }


    #endregion

    const string vehicleTag = "Vehicle";
    const string tileTag = "Tile";
    const string bonusVehcicleTag = "Bonus";

    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask groundLayer;

    public float horizRange = 80f;

    public float moveSpeed;
    [SerializeField] private Transform initalPoint;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float inputHoldTime = 0.2f;

    public Mover mover;
    public float variableJumpHeightMultiplier = 0.31f;
    public float coyoteTime = -3f;
    public float releaseForceMagnitude = 10f;

    public float stompForce = 20f;
    private bool isStomping = false;
    public LineRenderer line;


    public float swingForce = 4.5f;         // Force applied while swinging
    public float swingDamper = 7f;          // Dampening for the spring during the swing
    public float maxSwingDistance = 10f;    // Maximum distance for the spring joint
    public float animationDuration = 3f;
    public Transform playerToungeTransform;
    public Transform currentEnemy { get; set; }

    public Rigidbody rb => GetComponent<Rigidbody>();
    public SpringJoint springJoint { get; set; }        // Spring joint for swinging

    public void UseJumpInput() => JumpInput = false;
    public void UseStompInput() => StompInput = false;
    public void UseGrappleInput() => swingInput = false;

    public bool swingInput { get; private set; } 
    public bool JumpInput { get; private set; }
    public bool StompInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool lockedOn { get; private set; }

    private float jumpInputStartTime;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        playerDead = false;
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        InAirState = new PlayerInAirState(this, StateMachine);
        LandState = new PlayerLandState(this, StateMachine);
        StompState = new PlayerStompState(this, StateMachine);
        SwingState = new PlayerSwingState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        Debug.Log("the current state machine " + StateMachine.CurrentState);
        CheckPlayerJumpInput();
        CheckJumpInputHoldTime();
        CheckPlayerStompInput();
        CheckPlayerSwingInput();

    }

    private void CheckPlayerSwingInput()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            swingInput = true;
        }
    }

    private void CheckPlayerStompInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StompInput = true;
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void CheckPlayerJumpInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            JumpInputStop = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isStomping && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DestroyObstructions();
            setIsStomping(false);
        }
    }


    public bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(initalPoint.transform.position, Vector3.down, out hit, raycastDistance, groundLayer)) 
        {
            return true;
            
        }
        return false;
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    void DestroyObstructions()
    {
        // Implement logic to destroy nearby obstructions
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);  // Detect nearby objects
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(bonusVehcicleTag))
            {
                Destroy(hitCollider.gameObject);
                Debug.Log("Obstruction destroyed!");
            }
        }
    }
    public void OnDestroyJoint()
    {
        SpringJoint joint = GetComponent<SpringJoint>();

        if (joint != null && !joint.IsDestroyed())
        {
            Destroy(joint.GetComponent<SpringJoint>());
        }

    }

    public bool getIsStomping()
    {
        return isStomping;
    }

    public void setIsStomping(bool value)
    {
        isStomping = value;
    }

    public void setIsLockedOn(bool value)
    {
        lockedOn = value;
    }

    public bool getIsLockedOn()
    {
        return lockedOn;
    }

    // This method is called by Unity Editor to draw gizmos for visualization
    void OnDrawGizmos()
    {
        // Set the gizmo color to green if grounded, red if not
        Gizmos.color = IsGrounded() ? Color.green : Color.red;

        // Draw a line representing the raycast
        Gizmos.DrawLine(initalPoint.transform.position, initalPoint.transform.position + Vector3.down * raycastDistance);

        // Optionally, draw a small sphere at the raycast endpoint for clarity
        Gizmos.DrawWireSphere(initalPoint.transform.position + Vector3.down * raycastDistance, 0.1f);
    }

    //TODO - Develop a scoring system (Ideas: count how many tiles have passed, how far the player has moved through in the world, or how long they've survived, etc)
    //TODO - Handle player getting hit by a vehicle (Ideas: reload the level, add a "lose" screen to show the score, etc.)


}

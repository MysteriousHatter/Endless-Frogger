using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingState : PlayerAbilityState
{

    private Transform swingTarget;         // The object the player swings from
    private Vector3 swingAnchorPoint;       // The point where the player attaches

    bool isDrawing = false;
    private float distanceAnimation;
    private float counter;
    bool finishedDrawing = false;
    Vector3 curentEndpointPosition;

    public PlayerSwingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InitializeSwing();    // Initialize the swing logic when entering this state
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        DrawLine();
        // Check for input to swing or cancel swing
        HandleSwingControls();

        // If jump is pressed, detach from the swing
        if (player1.JumpInput)
        {
            player1.mover.SetMoveZAxis(true);
            DetachSwing();
        }
    }

    // Initialize the swing when the player connects to the swingable object
    private void InitializeSwing()
    {
        // Assuming the swingTarget is set when the player grapples to an object
        swingTarget = player1.currentEnemy; // Replace with actual logic to find the target
        player1.mover.SetMoveZAxis(false);
        if (swingTarget == null) return;

        // Add a SpringJoint to the player and configure it for swinging
        player1.springJoint = player1.gameObject.AddComponent<SpringJoint>();
        player1.springJoint.autoConfigureConnectedAnchor = false;
        player1.springJoint.spring = player1.swingForce;
        player1.springJoint.damper = player1.swingDamper;

        // Reduce the max distance to make the player closer to the swing point
        float shortenedDistance = player1.maxSwingDistance * 0.09f;  // 60% of the original max distance
        player1.springJoint.maxDistance = shortenedDistance;

        // Optionally set the minimum distance to prevent the player from moving too far away
        player1.springJoint.minDistance = shortenedDistance * 0.8f; // Adjust to your liking

        // Attach the swingTarget (anchor point) to the SpringJoint
        player1.springJoint.connectedBody = swingTarget.GetComponent<Rigidbody>();

        // Set the anchor point on the target for the SpringJoint to connect to
        swingAnchorPoint = swingTarget.transform.InverseTransformPoint(swingTarget.transform.position);
        player1.springJoint.connectedAnchor = swingAnchorPoint;

        distanceAnimation = Vector3.Distance(player1.springJoint.transform.position, player1.springJoint.connectedAnchor);
        isDrawing = true;

        player1.line.enabled = true;  // Show the grapple line (optional)
    }

    // Handle player input for swinging (e.g., applying force in specific directions)
    private void HandleSwingControls()
    {
        if(player1.springJoint != null) 
        {
            // Calculate pendulum motion based on player's current position relative to the swing anchor point
            Vector3 playerPostion = player1.playerToungeTransform.position;
            Vector3 anchorPosition = player1.springJoint.connectedBody.transform.position;

            //Calculate the swing force along the Z-axis
            float distanceFromAnchorZ = playerPostion.z - anchorPosition.z;
            float pendulumForce = -distanceFromAnchorZ * player1.swingForce; // Force that pulls player back towards the center (pendulum effect)

            // Apply the calculated force along the Z-axis to simulate the pendulum effect
            player1.rb.AddForce(new Vector3(0f, 0f, pendulumForce * Time.deltaTime), ForceMode.Acceleration);

            // Optionally: Limit the max speed to simulate realistic pendulum swing behavior
            float maxSwingSpeed = 7f; // Adjust this value as needed
            if (Mathf.Abs(player1.rb.velocity.z) > maxSwingSpeed)
            {
                player1.rb.velocity = new Vector3(player1.rb.velocity.x, player1.rb.velocity.y, Mathf.Sign(player1.rb.velocity.z) * maxSwingSpeed);
            }

            // Debugging to see the swing force applied
            Debug.Log("Pendulum swing force: " + pendulumForce);
        }
    }

    // Detach from the swing by removing the SpringJoint and resetting player state
    private void DetachSwing()
    {
        if (player1.springJoint != null)
        {
            player1.OnDestroyJoint(); // Remove the SpringJoint
            player1.springJoint = null;   // Reset the referenc
            player1.line.enabled = false;  // Hide the grapple line (optional)e
          
        }

        

        // Optionally add a jump-like force to simulate "releasing" from the swing
        Vector3 releaseDirection = player1.rb.velocity.normalized;
        //player1.rb.AddForce(releaseDirection * player1.releaseForceMagnitude, ForceMode.Impulse);
        player1.UseJumpInput();
        player1.UseGrappleInput();
        // Change state to in-air state to simulate the player continuing to move after the swing
        stateMachine.ChangeState(player1.InAirState);
        Debug.Log("CHange state to air state");

    }

    private void DrawLine()
    {
        if (!isDrawing) return; // Return early if not drawing

        if (counter < distanceAnimation)
        {
            Debug.Log("Still Drawing");
            finishedDrawing = false;
            counter += (Time.deltaTime / player1.animationDuration) * 5f; // Use Time.deltaTime for frame-rate independence

            // Calculate how far along the line we are
            float lerpFactor = Mathf.Lerp(0, distanceAnimation, counter);

            // Define start and end points
            Vector3 pointA = player1.playerToungeTransform.transform.position;
            Vector3 pointB = swingTarget.transform.position;

            // Calculate the position along the line using Lerp
            Vector3 pointAlongLine = Vector3.Lerp(pointA, pointB, lerpFactor);

            // Update the LineRenderer's positions

            player1.line.SetPosition(0, pointA);        // Start at player's hand
            player1.line.SetPosition(1, pointAlongLine); // Line moves towards target point
        }
        else
        {
            finishedDrawing = true;
            
            player1.line.SetPosition(0, player1.playerToungeTransform.transform.position);
            player1.line.SetPosition(1, swingTarget.transform.position);
            
            //PullPlayer(); // Since there's only a swing mechanic, directly call PullPlayer when the line finishes drawing
        }
    }

    public override void Exit()
    {
        base.Exit();
        //stateMachine.ChangeState(player1.InAirState);
        //DetachSwing();  // Ensure the player detaches from the swing when exiting the state
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMoveState : PlayerGroundedState

{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        PlayerHorizontalMovement();


        if (!isExitingState)
        {
            if (player1.rb.velocity.magnitude <= 0f && zInput == 0)
            {
                stateMachine.ChangeState(player1.IdleState);
            }
        }
    }

    private void PlayerHorizontalMovement()
    {
        float xMove = Input.GetAxis("Horizontal");

        // Calculate movement velocity
        float horizVelocity = xMove * player1.moveSpeed;

        // Get current position and clamp the x-axis
        float clampedHorizPos = Mathf.Clamp(player1.transform.position.x, -player1.horizRange, player1.horizRange);

        // Apply the velocity to the Rigidbody
        player1.rb.velocity = new Vector3(horizVelocity, player1.rb.velocity.y, 0f);
        Debug.Log("The player's velocity " + player1.rb.velocity.magnitude);

        // Prevent the Rigidbody from moving outside the allowed horizontal range
        if (player1.transform.position.x < -player1.horizRange || player1.transform.position.x > player1.horizRange)
        {
            // Manually clamp the player's position if it exceeds the horizontal range
            player1.rb.position = new Vector3(clampedHorizPos, player1.rb.position.y, player1.rb.position.z);
        }

        // Determine the target local rotation based on movement direction
        Quaternion targetRotation;

        if (player1.rb.velocity.x > 0)
        {
            // Rotate player to face right with a local tilt on the Y-axis
            targetRotation = Quaternion.AngleAxis(90f, Vector3.up); // Tilt right around local Y-axis
        }
        else if (player1.rb.velocity.x < 0)
        {
            // Rotate player to face left with a local tilt on the Y-axis
            targetRotation = Quaternion.AngleAxis(-90f, Vector3.up); // Tilt left around local Y-axis
        }
        else
        {
            // Reset rotation to face forward with no tilt
            targetRotation = Quaternion.identity; // No rotation (facing forward)
        }

        // Smoothly rotate towards the target local rotation using RotateTowards
        player1.transform.localRotation = Quaternion.RotateTowards(player1.transform.localRotation, targetRotation, 450f * Time.deltaTime);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

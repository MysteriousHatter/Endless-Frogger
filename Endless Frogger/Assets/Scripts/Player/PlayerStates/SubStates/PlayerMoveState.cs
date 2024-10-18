using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMoveState : PlayerGroundedState

{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

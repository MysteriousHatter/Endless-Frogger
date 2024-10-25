using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{

    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //core.Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            // Check if the player's Rigidbody velocity along the X-axis is greater than a small threshold
            if (player1.rb.velocity.magnitude != 0 || zInput != 0)
            {
                Debug.Log("Player is moving with velocity: " + player1.rb.velocity.x);
                stateMachine.ChangeState(player1.MoveState); // Change to move state
            }

            // If the velocity is small enough, you can assume the player is idle
            else
            {
                Debug.Log("Player is not moving");
            }
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

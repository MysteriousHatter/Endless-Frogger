using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerGroundedState : PlayerState
{
    protected float zInput;

    private bool JumpInput;
    private bool PressGrappleInput;
    private bool isGrounded;
    private bool stompInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player1.IsGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        player1.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        JumpInput = player1.JumpInput;
        zInput = player1.mover.getZMove();
        //HoldingGrappleInput = player.InputHandler.isHoldingGrappleButton;


        if (JumpInput && player1.JumpState.CanJump())
        {
            Debug.Log("Can jump");
            stateMachine.ChangeState(player1.JumpState);
        }
        else if (!isGrounded)
        {
            player1.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player1.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}

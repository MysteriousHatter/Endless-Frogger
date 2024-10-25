using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            //if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
            //{
            //    Debug.Log("We are grounded");
            //    stateMachine.ChangeState(player.IdleState);
            //}
            //else
            //{
            //    stateMachine.ChangeState(player.InAirState);
            //}
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

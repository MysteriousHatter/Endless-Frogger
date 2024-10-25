using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerStompState : PlayerAbilityState
{

    public PlayerStompState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player1.setIsStomping(true);
        player1.UseStompInput();
        player1.rb.velocity = new Vector3(0, -player1.stompForce, 0);
        isAbilityDone = true;
        stateMachine.ChangeState(player1.LandState);
        //player.GrappleDirectionalState.ResetCanGrapple();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}

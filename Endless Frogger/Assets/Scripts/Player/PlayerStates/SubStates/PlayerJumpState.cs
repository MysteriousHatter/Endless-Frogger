using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        amountOfJumpsLeft = 1;
    }

    public override void Enter()
    {
        base.Enter();
        player1.UseJumpInput();
        player1.Jump();
        isAbilityDone = true;
        amountOfJumpsLeft--;
        player1.InAirState.SetIsJumping();
        stateMachine.ChangeState(player1.InAirState);

    }
    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = 1;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}

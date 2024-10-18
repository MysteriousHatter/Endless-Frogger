using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerState
{

    protected PlayerStateMachine stateMachine;
    protected Player player1;
    protected bool isExitingState;
    protected float startTime;


    public PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.player1 = player;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }


}

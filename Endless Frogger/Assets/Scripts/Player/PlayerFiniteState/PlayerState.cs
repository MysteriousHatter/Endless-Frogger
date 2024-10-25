using JetBrains.Annotations;
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

    private string animBoolName;
    protected bool isAnimationFinished;


    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        this.player1 = player;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        player1.Anim.SetBool(animBoolName, true);
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player1.Anim.SetBool(animBoolName, false);
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

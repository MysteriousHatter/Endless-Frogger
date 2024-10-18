using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (player1.transform.position.x != 0 || player1.mover.getZMove() != 0)
            {
                stateMachine.ChangeState(player1.MoveState);
            }
            else if (player1.transform.position.x == 0 || player1.mover.getZMove() == 0)
            {
                stateMachine.ChangeState(player1.IdleState);
            }
        }
    }
}

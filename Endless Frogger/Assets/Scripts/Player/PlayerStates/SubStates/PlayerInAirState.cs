using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerInAirState : PlayerState
{
    //Input
    private float xInput;
    private float zInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;


    //Checks
    private bool isGrounded;
    private bool tappingSwingInput;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player1.IsGrounded();
        Debug.Log("We are grounded " + isGrounded);

    }

    public override void Enter()
    {
        base.Enter();
        //player.GrappleDirectionalState.ResetCanGrapple();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        Debug.Log("Are we jumping " + isJumping);
        Debug.Log("Are we grounded " + isGrounded);
        jumpInput = player1.JumpInput;
        jumpInputStop = player1.JumpInputStop;
        tappingSwingInput = player1.swingInput;
        CheckJumpMultiplier();
        PlayerHorizontalMovement();

        if (isGrounded)
        {
            Debug.Log("We have landed");
            stateMachine.ChangeState(player1.LandState);
        }
        else if (jumpInput && player1.JumpState.CanJump())
        {
            stateMachine.ChangeState(player1.JumpState);
        }
        else if(player1.StompInput && (!isGrounded && !player1.getIsStomping()))
        {
            stateMachine.ChangeState(player1.StompState);
        }
        else if (player1.getIsLockedOn() && tappingSwingInput)
        {
            stateMachine.ChangeState(player1.SwingState);
        }

    }


    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                if(player1.rb.velocity.y > 0f)
                {
                    player1.rb.velocity = new Vector3(player1.rb.velocity.x, player1.rb.velocity.y * player1.variableJumpHeightMultiplier, player1.rb.velocity.z);
                }
                isJumping = false;
            }
            else if (player1.rb.velocity.y <= 0f)
            {
                isJumping = false;
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
        xInput = clampedHorizPos;

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

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + player1.coyoteTime)
        {
            coyoteTime = false;
            player1.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;


    public void SetIsJumping() => isJumping = true;
}

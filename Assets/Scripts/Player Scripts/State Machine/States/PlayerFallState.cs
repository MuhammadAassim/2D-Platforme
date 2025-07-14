using UnityEngine;

public class PlayerFallState : PlayerState // Fall state jo PlayerState base class se inherit karti hai
{
    public PlayerFallState(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) : base(playerMovement, playerStateMachine)
    {
        // Constructor mein PlayerMovement aur StateMachine ka reference pass ho raha hai
    }

    public override void EnterState()
    {
        base.EnterState(); // Parent class ka EnterState method call kar rahe hain (agar common logic ho to chalega)

        playerMovement.Animator.Play("Fall");
    }

    public override void ExitState()
    {
        base.ExitState(); // Parent class ka ExitState method call kar rahe hain
    }

    public override void FrameUpdate()
    {
        // Agar player ground ko touch kare to idle state mein chala jaye
        if (playerMovement.IsGrounded)
        {
            playerStateMachine.ChangeState(playerMovement.idleState); // Idle state mein switch kar rahe hain
        }

        // Agar player space press kare aur double jump available ho to double jump state mein chala jaye
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.CanDoubleJump)
        {
            playerStateMachine.ChangeState(playerMovement.doubleJumpState); // Double jump ke liye state change
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // Physics-related updates base class se inherit kar rahe hain
    }
}
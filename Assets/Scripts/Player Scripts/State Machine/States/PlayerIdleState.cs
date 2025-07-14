using UnityEngine;

public class PlayerIdleState : PlayerState // Idle state jo PlayerState class se inherit karti hai
{
    public PlayerIdleState(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) : base(playerMovement, playerStateMachine)
    {
        // Constructor base class ko playerMovement aur stateMachine pass karta hai
    }

    public override void EnterState()
    {
        base.EnterState(); // Parent class ka EnterState call kar raha hai (agar koi common logic ho)

        playerMovement.Animator.Play("Idle");
    }

    public override void ExitState()
    {
        base.ExitState(); // Parent class ka ExitState call kar raha hai (agar koi common logic ho)
    }

    public override void FrameUpdate()
    {
        // Agar horizontal input 0.1 se zyada ho to player run state mein chala jaye
        if (Mathf.Abs(playerMovement.HorInput) > 0.1f)
        {
            playerStateMachine.ChangeState(playerMovement.runState); // Running state mein transition ho rahi hai
        }

        // Agar space press ho aur player ground par ho to jump state mein chala jaye
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.IsGrounded)
        {
            playerStateMachine.ChangeState(playerMovement.jumpState); // Jump state mein transition
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // Parent class ka PhysicsUpdate call ho raha hai
    }
}
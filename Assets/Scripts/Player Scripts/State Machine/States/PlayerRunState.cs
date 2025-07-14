using UnityEngine;

public class PlayerRunState : PlayerState // Run state jo PlayerState base class se inherit karti hai
{
    public PlayerRunState(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) : base(playerMovement, playerStateMachine)
    {
        // Constructor base class ko playerMovement aur stateMachine pass karta hai
    }

    public override void EnterState()
    {
        base.EnterState(); // EnterState ka base implementation call karta hai (agar kuch common logic ho)

        playerMovement.Animator.Play("Run");
    }

    public override void ExitState()
    {
        base.ExitState(); // ExitState ka base implementation call karta hai
    }

    public override void FrameUpdate()
    {
        // Agar player ka horizontal input 0.1 se kam ho jaye, to idle state mein chala jaye
        if (Mathf.Abs(playerMovement.HorInput) < 0.1f)
        {
            playerStateMachine.ChangeState(playerMovement.idleState); // Idle state mein transition ho rahi hai
        }

        // Agar player space press kare aur woh ground par ho to jump state mein chala jaye
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.IsGrounded)
        {
            playerStateMachine.ChangeState(playerMovement.jumpState); // Jump state mein transition ho rahi hai
        }

        // Player ki movement ko run ke acceleration aur deceleration ke saath handle karna
        playerMovement.Move(playerMovement.MovementSO.groundAcceleration, playerMovement.MovementSO.groundDeceleration);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // PhysicsUpdate ka base implementation call ho raha hai
    }
}
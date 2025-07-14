using UnityEngine;

public class PlayerDoubleJump : PlayerState // Double Jump state jo PlayerState base class ko inherit karti hai
{
    public PlayerDoubleJump(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) : base(playerMovement, playerStateMachine)
    {
        // Constructor base class ko playerMovement aur stateMachine pass karta hai
    }

    public override void EnterState()
    {
        // Agar gravity flipped hai to double jump force upar hogi warna neeche
        float doubleJumpForce = playerMovement.IsFlipped
            ? -playerMovement.MovementSO.doubleJumpHeight : playerMovement.MovementSO.doubleJumpHeight;

        // Rigidbody ko double jump force de rahe hain
        playerMovement.Rb.velocity = new Vector2(playerMovement.Rb.velocity.x, doubleJumpForce);

        // Jump ka count 1 barh gaya
        playerMovement.JumpUsed++;

        // Ab dobara double jump allow nahi hai
        playerMovement.CanDoubleJump = false;

        playerMovement.Animator.Play("Double Jump");
    }

    public override void ExitState()
    {
        base.ExitState(); // Parent class ka exit state call ho raha hai (agar common logic ho)
    }

    public override void FrameUpdate()
    {
        // Agar player neeche gir raha hai to fall state mein chala jaye
        if (playerMovement.Rb.velocity.y < 0f)
        {
            playerStateMachine.ChangeState(playerMovement.fallState); // Fall state mein switch ho raha hai
        }

        // Agar dobara space press karein aur double jump allow ho to wapas double jump karein (failsafe)
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.CanDoubleJump)
        {
            playerStateMachine.ChangeState(playerMovement.doubleJumpState); // Wapas double jump state mein jaye
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // Physics update base class se call ho raha hai
    }
}
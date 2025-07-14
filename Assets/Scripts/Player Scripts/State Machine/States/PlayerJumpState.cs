using UnityEngine;

public class PlayerJumpState : PlayerState // Jump state jo base PlayerState se inherit karti hai
{
    public PlayerJumpState(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) : base(playerMovement, playerStateMachine)
    {
        // Constructor base class ko playerMovement aur stateMachine pass karta hai
    }

    public override void EnterState()
    {
        // Agar gravity flipped hai to jump force upar lagay gi warna neeche
        float jumpForce = playerMovement.IsFlipped ? -playerMovement.MovementSO.jumpHeight : playerMovement.MovementSO.jumpHeight;

        // Rigidbody ko vertical direction mein force de rahe hain
        playerMovement.Rb.velocity = new Vector2(playerMovement.Rb.velocity.x, jumpForce);

        // Jump ka count 1 barh gaya
        playerMovement.JumpUsed++;

        playerMovement.Animator.Play("Jump");
    }

    public override void ExitState()
    {
        base.ExitState(); // Agar parent class mein koi extra logic ho to wo run karega
    }

    public override void FrameUpdate()
    {
        // Agar vertical velocity neeche ja rahi hai to fall state mein chala jaye
        if (playerMovement.Rb.velocity.y < 0)
        {
            playerStateMachine.ChangeState(playerMovement.fallState); // Fall state mein switch ho raha hai
        }

        // Agar Space dobara press karein aur double jump allowed ho to double jump state mein chalayin
        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.CanDoubleJump)
        {
            playerStateMachine.ChangeState(playerMovement.doubleJumpState); // Double jump ke liye state change
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); // Physics-related update parent class se call ho raha hai
    }
}
public class PlayerState // Ye base class hai har ek player state ke liye (jaise jump, run, idle etc.)
{
    protected PlayerMovement playerMovement; // PlayerMovement ka reference taake movement aur inputs ka access ho
    protected PlayerStateMachine playerStateMachine; // FSM ka reference taake state switch kar sakein

    public PlayerState(PlayerMovement playerMovement, PlayerStateMachine playerStateMachine) // Constructor jo references set karta hai
    {
        this.playerMovement = playerMovement; // PlayerMovement object assign kiya
        this.playerStateMachine = playerStateMachine; // PlayerStateMachine object assign kiya
    }

    public virtual void EnterState()
    { } // Jab state start ho to ye method override hoke chalega

    public virtual void ExitState()
    { } // Jab state se bahar nikle to ye method override hoke chalega

    public virtual void FrameUpdate()
    { } // Har frame mein logic update karne ke liye override hoga

    public virtual void PhysicsUpdate()
    { } // Physics-related updates ke liye override karna hoga
}
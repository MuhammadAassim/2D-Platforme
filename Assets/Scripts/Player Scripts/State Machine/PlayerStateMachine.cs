public class PlayerStateMachine // Ye class FSM (Finite State Machine) ka main controller hai
{
    public PlayerState CurrentState { get; set; } // Abhi jo current active state hai uska reference

    public void Initialize(PlayerState startingState) // FSM ko kisi starting state ke saath initialize karte hain
    {
        CurrentState = startingState; // Starting state ko set karte hain
        CurrentState.EnterState(); // Aur us state ka enter function chala dete hain
    }

    public void ChangeState(PlayerState newState) // Jab state switch karni ho to ye function use hota hai
    {
        CurrentState.ExitState(); // Current state ka exit logic run hota hai
        CurrentState = newState; // New state ko set karte hain
        CurrentState.EnterState(); // New state ka enter logic chalte hain
    }
}
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scriptable Object Settings")]
    [SerializeField] private PlayerMovementSO playerMovementSO; // Scriptable Object jo movement settings rakhta hai

    [Header("RigidBody2D")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody2D component player ke physics control ke liye

    [Header("Animator")]
    [SerializeField] private Animator animator; // Animation control ke liye animator reference

    [Header("Colliders")]
    [SerializeField] private Collider2D headCollider; // Head collider ground ya ceiling detect karne ke liye

    [SerializeField] private Collider2D feetCollider; // Feet collider ground detect karne ke liye

    // Internal states
    private bool isFacingRight = true; // Player right side face kar raha hai ya nahi

    private bool isFlipped = false; // Gravity flip hua hai ya nahi
    private bool isGrounded; // Player ground par hai ya nahi
    private bool canDoubleJump; // Double jump allowed hai ya nahi
    private int jumpUsed; // Kitni baar jump use kiya gaya

    private Vector2 moveVelocity; // Movement ka velocity vector
    private Vector2 moveDir; // Movement ka direction vector
    private float horDir; // Horizontal input direction

    // --- Public properties for FSM ---
    public Rigidbody2D Rb => rb; // Rigidbody ka public accessor

    public Animator Animator => animator; // Animator ka public accessor
    public PlayerMovementSO MovementSO => playerMovementSO; // Movement settings ka accessor
    public bool IsFacingRight => isFacingRight; // Player ka face direction ka check
    public bool IsFlipped => isFlipped; // Gravity flipped hai ya nahi
    public bool IsGrounded => isGrounded; // Grounded status ka public accessor
    public bool CanDoubleJump { get => canDoubleJump; set => canDoubleJump = value; } // Double jump ka getter/setter
    public int JumpUsed { get => jumpUsed; set => jumpUsed = value; } // Jump count ka getter/setter
    public float HorInput => horDir; // Horizontal input ka public value
    public Vector2 MoveDirection => moveDir; // Movement direction ka value
    public Vector2 MoveVelocity => moveVelocity; // Movement velocity ka value

    public void Move(float acceleration, float deceleration) => HandleMovement(acceleration, deceleration); // Move function jo movement handle karega

    // --- State Machine ---
    public PlayerStateMachine stateMachine { get; set; } // FSM ka reference

    public PlayerIdleState idleState { get; set; } // Idle state ka reference
    public PlayerRunState runState { get; set; } // Run state ka reference
    public PlayerJumpState jumpState { get; set; } // Jump state ka reference
    public PlayerDoubleJump doubleJumpState { get; set; } // Double jump state ka reference
    public PlayerFallState fallState { get; set; } // Fall state ka reference

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(); // FSM ka naya instance banaya

        idleState = new PlayerIdleState(this, stateMachine); // Idle state assign ki
        runState = new PlayerRunState(this, stateMachine); // Run state assign ki
        jumpState = new PlayerJumpState(this, stateMachine); // Jump state assign ki
        doubleJumpState = new PlayerDoubleJump(this, stateMachine); // Double jump state assign ki
        fallState = new PlayerFallState(this, stateMachine); // Fall state assign ki

        if (rb == null)
            rb = GetComponent<Rigidbody2D>(); // Rigidbody null ho to component se le lo

        jumpUsed = playerMovementSO.numbersOfJump; // Jump count ko initial value di
    }

    private void Start()
    {
        stateMachine.Initialize(idleState); // FSM ko idle state se initialize kiya
    }

    private void Update()
    {
        horDir = Input.GetAxis("Horizontal"); // Horizontal input read karo
        moveDir = new Vector2(horDir, 0f).normalized;

        GravityShift(); // Gravity ko flip karna

        stateMachine.CurrentState.FrameUpdate(); // Frame-based FSM logic

        if (transform.position.y < -20f)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate(); // Physics update function har physics frame me chalayega

        if (isGrounded)
        {
            Move(playerMovementSO.groundAcceleration, playerMovementSO.groundDeceleration); // Ground par hone par move karna
        }
        CollisionCheck(); // GROUND CHECK PEHLE KARO
    }

    // ------------------------------

    #region Movement

    private void HandleMovement(float acceleration, float deceleration)
    {
        if (moveDir != Vector2.zero)
        {
            TurnCheck(moveDir); // Movement direction ke hisab se turn check

            float speed = Input.GetKey(KeyCode.LeftShift) ? playerMovementSO.maxRunSpeed : playerMovementSO.maxWalkSpeed; // Shift dabane par run, warna walk
            Vector2 targetVelocity = new Vector2(moveDir.x, 0f) * speed; // Target velocity banayi

            float targetXVelocity = Mathf.Clamp(moveDir.x * speed, -playerMovementSO.maxRunSpeed, playerMovementSO.maxRunSpeed); // X velocity clamp kiya max limit se

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.deltaTime); // Velocity ko smoothly change kiya
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y); // Rigidbody ko new velocity di
        }
        else
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.deltaTime); // Agar movement nahi hai to slow stop
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y); // Rigidbody ka x velocity update
        }
    }

    #endregion Movement

    // ------------------------------

    #region Turning

    private void TurnCheck(Vector2 moveDir)
    {
        if (moveDir.x > 0 && !isFacingRight)
        {
            Turn(true); // Right face nahi kar raha aur right move kar raha to turn
        }
        else if (moveDir.x < 0 && isFacingRight)
        {
            Turn(false); // Left move kar raha aur right face kar raha to turn
        }
    }

    private void Turn(bool faceRight)
    {
        isFacingRight = faceRight; // Facing direction update ki

        Vector3 scale = transform.localScale; // Current scale le li
        scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1f : -1f); // X scale ko flip ya normal kiya
        transform.localScale = scale; // Final scale apply kiya
    }

    #endregion Turning

    // ------------------------------

    #region Ground Check

    private void IsGroundedCheck()
    {
        Vector2 direction = isFlipped ? Vector2.up : Vector2.down; // Flip hone par raycast ka direction change
        Vector2 origin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y); // Raycast origin feet se
        Vector2 size = new Vector2(feetCollider.bounds.size.x, playerMovementSO.groundDetectionCheckRay); // Raycast size

        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, direction, playerMovementSO.groundDetectionCheckRay, playerMovementSO.groundMask); // BoxCast ground detect karne ke liye
        isGrounded = hit.collider != null; // Ground mila ya nahi

        if (isGrounded)
        {
            jumpUsed = 0; // Ground par hai to jump reset
            canDoubleJump = true; // Double jump allow kar diya
        }
    }

    private void CollisionCheck()
    {
        IsGroundedCheck(); // Ground check function call kiya
    }

    #endregion Ground Check

    // ------------------------------

    #region Gravity Shift

    private void GravityShift()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 scale = transform.localScale; // Current scale le li

            if (!isFlipped)
            {
                rb.gravityScale = playerMovementSO.flippedGravityScale * playerMovementSO.gravityMultiplier; // Gravity ko ulta kiya
                scale.y *= -1; // Player ka Y scale flip kiya
                isFlipped = true; // Flipped flag true
            }
            else
            {
                rb.gravityScale = playerMovementSO.normalGravityScale * playerMovementSO.gravityMultiplier; // Normal gravity apply kiya
                scale.y = Mathf.Abs(scale.y); // Scale ko wapas positive kiya
                isFlipped = false; // Flipped flag false
            }

            transform.localScale = scale; // Final scale apply kiya
        }
    }

    #endregion Gravity Shift

    // ------------------------------

    #region Death

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Traps"))
        {
            Death();
        }
    }

    private void Death()
    {
        // Pehle vertical velocity zero karo taake force effect properly ho
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        // Upar ki taraf ek impulsive force laga do
        rb.AddForce(Vector2.up * playerMovementSO.bounceForce, ForceMode2D.Impulse);

        animator.SetTrigger("Death");
    }

    private void DeathEnd()
    {
        Destroy(gameObject);
    }


    #endregion
}
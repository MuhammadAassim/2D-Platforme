using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")] // Inspector mein "Player Movement" naam ka ScriptableObject banane ka option deta hai
public class PlayerMovementSO : ScriptableObject
{
    [Header("Walk")] // Inspector mein "Walk" section ka header dikhayega
    [Range(1f, 100f)]
    public float maxWalkSpeed; // Walk karne ke waqt max speed kitni hogi (slow chalne ke liye)

    [Range(0.25f, 50f)]
    public float groundAcceleration; // Ground pe move karte waqt acceleration kitni hogi

    [Range(0.25f, 50f)]
    public float groundDeceleration; // Ground pe rukne par deceleration kitna hoga

    [Header("Run")] // "Run" ke parameters ke liye header
    [Range(1f, 50f)]
    public float maxRunSpeed; // Shift press karne par player ki max run speed kya hogi

    [Header("Jumps")] // Jump se related parameters ke liye header
    [Range(1f, 50f)]
    public float jumpHeight; // Simple jump ki height

    [Range(1f, 50f)]
    public float doubleJumpHeight; // Double jump ki height

    [Range(1f, 5f)]
    public int numbersOfJump; // Total jumps allowed (1 = normal, 2 = double jump)

    [Range(1f, 100f)]
    public float bounceForce;

    [Header("Grounded/CollisionDetection")] // Ground detect karne se related settings ka header
    public LayerMask groundMask; // Kis layer ko ground samjha jaye (jaise "Ground" tag wali layer)

    public float groundDetectionCheckRay; // Raycast ki length jo ground check ke liye use hoti hai

    [Header("Gravity Settings")] // Gravity se related parameters ka header
    public int normalGravityScale; // Jab player normal hai to uski gravity ka scale

    public int flippedGravityScale; // Jab gravity flip hoti hai (upar girta hai) to uska gravity scale

    [Range(1f, 10f)]
    public int gravityMultiplier; // Extra gravity multiplier to tweak jump/fall speed
}
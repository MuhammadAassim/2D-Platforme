using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FallingTrap ek aisi trap hai jo ya to ground se girti hai ya roof se neeche uthti hai
public class FallingTrap : Trap
{
    // Do types ke trap define kar rahay hain: Ground se girne wala ya Roof se neeche aane wala
    public enum TrapType
    {
        Ground, // Zameen par rakha hua trap jo neeche girta hai
        Roof    // Chhat se neeche aane wala trap
    }

    [Header("TrapType")]
    [SerializeField] private TrapType trapType; // Inspector mein trap ka type set karne ke liye

    [Header("RigidBody2D")]
    [SerializeField] private Rigidbody2D rigidBody2D; // Trap ke object ka Rigidbody2D (taake force ya gravity apply ho sake)

    // Jab player trap ke trigger mein aaye to yeh function chalega
    public override void TriggerEffect(GameObject player)
    {
        switch (trapType) // Trap ka type check kar rahay hain
        {
            case TrapType.Ground:
                // Agar trap ground se girta hai
                if (rigidBody2D != null)
                {
                    rigidBody2D.gravityScale = 10; // Normal neeche girne ke liye gravity set kar rahe hain
                }
                break;

            case TrapType.Roof:
                // Agar trap roof se girta hai (ya upar se neeche aata hai)
                if (rigidBody2D != null)
                {
                    rigidBody2D.gravityScale = -10; // Negative gravity taake upar se neeche aaye
                }
                break;
        }
    }
}
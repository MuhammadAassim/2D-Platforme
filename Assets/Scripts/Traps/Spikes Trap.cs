using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : Trap
{
    // Spike ke 2 types: Ek rigidbody wale (physical force) aur ek animation wale (visual spikes)
    public enum SpikeType
    {
        RigidBody,     // Trap player ko force lagata hai
        Animation      // Trap khud animate hota hai (e.g. spikes pop up)
    }

    [SerializeField] private SpikeType spikeType; // Inspector mein spike type select karne ke liye
    [SerializeField] private Rigidbody2D rigidBody2D; // RigidBody force lagani hai
    [SerializeField] private Animator trapAnimator; // Animation trigger karne ke liye

    public override void TriggerEffect(GameObject player)
    {
        switch (spikeType)
        {
            case SpikeType.RigidBody:
                if (rigidBody2D != null)
                {
                    rigidBody2D.gravityScale = 10;
                }
                break;

            case SpikeType.Animation:
                // Agar animator mila to trigger karo animation
                if (trapAnimator != null)
                {
                    trapAnimator.SetTrigger("Activate"); // Animator mein "Activate" trigger hona chahiye
                }
                break;
        }
    }
}
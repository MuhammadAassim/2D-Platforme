using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrap : Trap
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    public override void TriggerEffect(GameObject player)
    {
        if (animator != null)
        {
            gameObject.SetActive(true);
            animator.SetTrigger("ON");
        }
    }
}
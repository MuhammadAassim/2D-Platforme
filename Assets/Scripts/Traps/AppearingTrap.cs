using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingTrap : Trap
{
    [Header("Trap")]
    [SerializeField] private GameObject trap;

    public override void TriggerEffect(GameObject player)
    {
        trap.SetActive(true);
    }
}

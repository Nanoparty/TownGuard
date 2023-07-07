using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public override State Run(StateManager sm)
    {
        Debug.Log("Dead");

        PerformDeath(sm);

        return this;
    }

    private void PerformDeath(StateManager sm)
    {
        sm.animator.Play("Death");
        sm.host.GetComponent<Collider>().enabled = false;
        sm.navAgent.destination = sm.host.transform.position;
        sm.weaponSlotManager.CloseRightDamageCollider();
        sm.healthBar.gameObject.SetActive(false);
        sm.host.layer = LayerMask.NameToLayer("Default");
    }
}

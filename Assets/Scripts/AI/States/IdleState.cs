using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IdleState : State
{
    public override State Run(StateManager sm)
    {
        UpdateMotion(sm);

        if (lookForTarget(sm))
        {
            return new FollowState();
        }

        return this;
    }

    private void UpdateMotion(StateManager sm)
    {
        float h = sm.navAgent.velocity.x;
        float v = sm.navAgent.velocity.z;
        float moveAmount = Mathf.Clamp(Mathf.Abs(h) + Mathf.Abs(v), 0, sm.maxSpeed);

        sm.animator.SetFloat("Horizontal", 0);
        sm.animator.SetFloat("Vertical", moveAmount);
    }

    private bool lookForTarget(StateManager sm)
    {
        List<GameObject> targets = new List<GameObject>();

        foreach(string tag in sm.targetTags)
        {
            targets.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

        targets = targets.Where(t => !t.GetComponent<Stats>().isDead).ToList();

        if (targets.Count == 0) return false;

        GameObject target = getClosestTarget(sm.host, targets);
        
        if (Vector3.Distance(sm.host.transform.position, target.transform.position) <= sm.targetDistance)
        {
            sm.target = target;
            Debug.Log("Found Target");

            return true;
        }

        return false;
    }

    private GameObject getClosestTarget(GameObject host, List<GameObject> targets)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach(GameObject t in targets)
        {
            float distance = Vector3.Distance(host.transform.position, t.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = t;
            }
        }

        return target;
    }
}

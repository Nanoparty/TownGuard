using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowState : State
{
    public override State Run(StateManager sm)
    {
        Debug.Log("Running Follow");

        sm.navAgent.destination = sm.target.transform.position;

        UpdateMotion(sm);

        if (TargetInRange(sm))
        {
            return new AttackState();
        }

        if (!DetectTarget(sm))
        {
            return new IdleState();
        }

        ReTarget(sm);

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

    private bool TargetInRange(StateManager sm)
    {
        return Vector3.Distance(sm.host.transform.position, sm.target.transform.position) <= sm.followDistance;
    }

    private bool DetectTarget(StateManager sm)
    {
        if (sm.target.GetComponent<Stats>().isDead)
            return false;
        return Vector3.Distance(sm.host.transform.position, sm.target.transform.position) <= sm.targetDistance;
    }

    private bool ReTarget(StateManager sm)
    {
        List<GameObject> targets = lookForTargets(sm);
        GameObject closestTarget = getClosestTarget(sm.host, targets);
        if (sm.target != closestTarget && Vector3.Distance(sm.target.transform.position, closestTarget.transform.position) >= sm.changeTargetDistance)
        {
            sm.target = closestTarget;
            return true;
        }
        return false;
    }

    private List<GameObject> lookForTargets(StateManager sm)
    {
        List<GameObject> targets = new List<GameObject>();

        foreach (string tag in sm.targetTags)
        {
            targets.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

        targets = targets.Where(t => !t.GetComponent<Stats>().isDead).ToList();

        return targets;
    }

    private GameObject getClosestTarget(GameObject host, List<GameObject> targets)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach (GameObject t in targets)
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

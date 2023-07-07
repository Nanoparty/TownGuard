using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackState : State
{
    DelayModule delayMod;

    public override State Run(StateManager sm)
    {
        Debug.Log("attack state");
        delayMod = sm.host.GetComponent<DelayModule>();

        

        if (!DetectTarget(sm))
        {
            return new IdleState();
        }
        if (ReTarget(sm))
        {
            return new FollowState();
        }
        if (!TargetInRange(sm))
        {
            return new FollowState();
        }
        

        sm.navAgent.destination = sm.host.transform.position;
        HandleRotation(sm);
        UpdateMotion(sm);

        PerformAttack(sm);
        
        return this;
    }

    private void PerformAttack(StateManager sm)
    {
        if (delayMod.active)
        {
            sm.navAgent.destination = sm.host.transform.position;
            sm.animator.Play("Attack 1");
            delayMod.StartDelay();
        }
        
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

    private void UpdateMotion(StateManager sm)
    {
        float h = sm.navAgent.velocity.x;
        float v = sm.navAgent.velocity.z;
        float moveAmount = Mathf.Clamp(Mathf.Abs(h) + Mathf.Abs(v), 0, sm.maxSpeed);

        sm.animator.SetFloat("Horizontal", 0);
        sm.animator.SetFloat("Vertical", moveAmount);
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

    private void HandleRotation(StateManager sm)
    {
        Vector3 targetDir = Vector3.zero;
        //float moveOverride = inputHandler.moveAmount;

        targetDir = sm.target.transform.position - sm.host.transform.position;
        targetDir.Normalize();
        targetDir.y = 0;

        float rs = 5f;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(sm.host.transform.rotation, tr, rs * Time.deltaTime);

        sm.host.transform.rotation = targetRotation;
    }
}

//Vector3 dir = currentLockOnTarget.transform.position - transform.position;
//dir.Normalize();
//dir.y = 0;

//Quaternion targetRotation = Quaternion.LookRotation(dir);
//transform.rotation = targetRotation;

//dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
//dir.Normalize();
//dir.y = 0;

//targetRotation = Quaternion.LookRotation(dir);
//Vector3 eulerAngle = targetRotation.eulerAngles;
//eulerAngle.y = 0;
//cameraPivotTransform.localEulerAngles = eulerAngle;

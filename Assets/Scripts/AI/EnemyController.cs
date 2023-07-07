using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navAgent;
    Animator animator;
    WeaponSlotManager weaponSlotManager;

    public bool isDead;
    public bool canAttack;

    public string state;

    public Transform navDestination;
    public WeaponItem weapon;

    public float stopDistance = 1f;
    public float maxSpeed = 1f;
    public float attackDelay = 2f;
    

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void Start()
    {
        weaponSlotManager.LoadWeaponOnSlot(weapon, false, null);

        state = "Move";
        canAttack = true;
    }

    private void Update()
    {
        if (isDead)
        {
            navAgent.destination = transform.position;
            return;
        }

        navDestination = GameObject.FindGameObjectWithTag("Player").transform;

        if (Vector3.Distance(transform.position, navDestination.position) <= stopDistance)
        {
            navAgent.destination = transform.position;
            state = "Attack";
        }
        else
        {
            navAgent.destination = navDestination.position;
            state = "Move";
            
        }

        if (state == "Attack")
        {
            animator.Play("Attack 1");
        }

        float h = navAgent.velocity.x;
        float v = navAgent.velocity.z;
        float moveAmount = Mathf.Clamp(Mathf.Abs(h) + Mathf.Abs(v), 0, maxSpeed);

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", moveAmount);
    }
}

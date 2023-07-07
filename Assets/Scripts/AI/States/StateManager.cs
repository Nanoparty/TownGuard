using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    public GameObject host;
    public GameObject target;

    public WeaponItem weapon;

    public float maxSpeed = 2f;

    public AIController controller;
    public AIHealthBar healthBar;
    public Animator animator;
    public WeaponSlotManager weaponSlotManager;
    public NavMeshAgent navAgent;
    public Stats stats;
    public PlayerManager playerManager;

    public State currentState;
    public string[] targetTags;

    public float targetDistance = 20f;
    public float followDistance = 1.5f;
    public float changeTargetDistance = 2f;
    public bool hit = false;
    public GameObject attacker;

    private void Awake()
    {
        host = gameObject;
        controller = GetComponent<AIController>();
        animator = GetComponent<Animator>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        navAgent = GetComponent<NavMeshAgent>();
        stats = GetComponent<Stats>();
        healthBar = GetComponentInChildren<AIHealthBar>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        weaponSlotManager.LoadWeaponOnSlot(weapon, false, targetTags);
        currentState = new IdleState();
    }

    private void Update()
    {
        if (playerManager.paused)
        {
            navAgent.destination = host.transform.position;
            animator.SetFloat("Vertical", 0);
            return;
        }

        if (host.GetComponent<Rigidbody>().IsSleeping())
        {
            host.GetComponent<Rigidbody>().WakeUp();
        }

        RunStateManager();
    }

    private void RunStateManager()
    {
        if (stats.isDead)
        {
            currentState = new DeathState();
        }

        if (hit && attacker != null)
        {
            target = attacker;
            currentState = new FollowState();
            hit = false;
            attacker = null;
        }

        State nextState = currentState?.Run(this);

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    public void UpdateHealth(float hp, float maxHp)
    {
        healthBar.SetHealth(hp, maxHp);
    }
}

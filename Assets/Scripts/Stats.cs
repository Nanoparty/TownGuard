using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    StateManager sm;

    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    public ParticleSystem BloodSplat;

    Animator animator;

    protected virtual void Awake()
    {
        sm = GetComponent<StateManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    protected int SetMaxHealthFromHealthLevel()
    {
        int maxHealth = healthLevel * 10;
        return maxHealth;
    }

    protected void PlayBloodSplat()
    {
        BloodSplat.Play();
    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth == 0) return;

        currentHealth = currentHealth - damage;

        PlayBloodSplat();

        sm.UpdateHealth(currentHealth, maxHealth);

        animator.Play("Damage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //animator.Play("Death");
            isDead = true;
        }
    }
}

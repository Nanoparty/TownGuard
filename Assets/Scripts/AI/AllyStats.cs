using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    Animator animator;
    AllyController allyController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        allyController = GetComponent<AllyController>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        int maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth == 0) return;

        currentHealth = currentHealth - damage;

        animator.Play("Damage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Death");
            allyController.isDead = true;
        }
    }
}

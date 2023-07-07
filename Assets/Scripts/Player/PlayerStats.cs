using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    public int maxStamina;
    public float currentStamina;
    public int staminaLevel = 10;

    public HealthBar healthBar;
    public StaminaBar staminaBar;

    public AnimatorHandler animatorHandler;
    public PlayerManager playerManager;
    public PlayerAudio playerAudio;

    public ParticleSystem HealEffect;

    public bool canStaminaRegen = true;
    public float staminaRegenRate = 1f;

    public float rollStaminaCost = 20;
    public float sprintStaminaCost = 1;
    public float jumpStaminaCost = 20;

    protected override void Awake()
    {
        base.Awake();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    protected override void Start()
    {
        base.Start();

        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar?.SetMaxStamina(maxStamina);
    }

    public void RegenStamina()
    {
        if (canStaminaRegen && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            staminaBar?.SetCurrentStamina((int)currentStamina);
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
    }

    public void DisableStaminaRegen()
    {
        StartCoroutine(StaminaDelay());
    }

    IEnumerator StaminaDelay()
    {
        canStaminaRegen = false;
        yield return new WaitForSeconds(1);
        canStaminaRegen = true;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth = currentHealth - damage;
        healthBar?.SetCurrentHealth(currentHealth);

        PlayBloodSplat();
        playerAudio.Groan();

        animatorHandler.PlayTargetAnimation("straight_sword_oh_hit_B_01", true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Death", true);
            isDead = true;
            playerManager.HandleDeath();
            Debug.Log("PLayer Dead");
            //TODO handle player death;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina -= damage;
        staminaBar?.SetCurrentStamina((int)currentStamina);
    }

    public void Heal(int amount)
    {
        HealEffect.Play();
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void Reset()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar?.SetMaxStamina(maxStamina);

        isDead = false;
    }
}

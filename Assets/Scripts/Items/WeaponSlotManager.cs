using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;

    public DamageCollider leftDamageCollider;
    public DamageCollider rightDamageCollider;

    Animator animator;

    QuickSlotManager quickSlotManager;

    public PlayerStats playerStats;
    public PlayerEffectsManager playerEffectsManager;
    InputHandler inputHandler;

    public WeaponItem attackingWeapon;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        quickSlotManager = FindObjectOfType<QuickSlotManager>();
        playerStats = GetComponent<PlayerStats>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot slot in weaponHolderSlots)
        {
            if (slot.isLeftHandSlot)
            {
                leftHandSlot = slot;
            }
            else if (slot.isRightHandSlot)
            {
                rightHandSlot = slot;
            }
        }
    }

    public void LoadNewestWeapon(string[] tags)
    {
        //WeaponItem weapon = 
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft, string[] tags)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            //quickSlotManager.UpdateWeaponQuickSlots(true, weaponItem);

            if(weaponItem != null)
            {
                animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
            }
        }
        else
        {
            //Debug.Log("Loading right weapon");
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider(tags);
            if (gameObject.tag == "Player")
            {
                quickSlotManager.UpdateWeaponQuickSlots(false, weaponItem);
            }

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
            }
        }
    }

    private void LoadLeftWeaponDamageCollider()
    {
        leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    private void LoadRightWeaponDamageCollider(string[] tags)
    {
        rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        if (rightDamageCollider != null)
        {
            rightDamageCollider.enemyTags.AddRange(tags);
        }
        //playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenRightDamageCollider()
    {
        rightDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftDamageCollider.DisableDamageCollider();
    }

    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }

    public void StopWeaponFX()
    {
        playerEffectsManager.StopWeaponFX(false);
    }

    public void SetRightWeaponTags(string[] tags)
    {
        rightDamageCollider.enemyTags.AddRange(tags);
    }

    public void SetLeftWeaponTags(string[] tags)
    {
        leftDamageCollider.enemyTags.AddRange(tags);
    }
}

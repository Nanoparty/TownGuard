using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    PlayerEffectsManager playerEffectsManager;

    public string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        inputHandler = GetComponent<InputHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }
            if (lastAttack == weapon.OH_Heavy_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_2, true);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (weapon.name == "Unarmed") return;
        weaponSlotManager.attackingWeapon = weapon;
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
        playerEffectsManager.PlayWeaponFX(false);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (weapon.name == "Unarmed") return;
        weaponSlotManager.attackingWeapon = weapon;
        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
        playerEffectsManager.PlayWeaponFX(false);
    }
}

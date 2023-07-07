using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerAudio playerAudio;
    PlayerStats playerStats;
    public Animator anim;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    WeaponSlotManager weaponSlotManager;
    int vertical;
    int horizontal;
    public bool canRotate;

    public GameObject potionBottle;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerAudio = GetComponentInParent<PlayerAudio>();
        playerStats = GetComponentInParent<PlayerStats>();
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        weaponSlotManager = GetComponentInParent<WeaponSlotManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical

        float v = 0f;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if(verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }

        #endregion

        #region Horizontal

        float h = 0f;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationInstant(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.05f);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }

    public bool IsPlaying(string animation)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animation);
    }

    public void OpenRightDamageCollider()
    {
        weaponSlotManager.rightDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        weaponSlotManager.leftDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        weaponSlotManager.rightDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        weaponSlotManager.leftDamageCollider.DisableDamageCollider();
    }

    public void DrainStaminaLightAttack()
    {
        weaponSlotManager.playerStats.TakeStaminaDamage(Mathf.RoundToInt(weaponSlotManager.attackingWeapon.baseStamina * weaponSlotManager.attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        weaponSlotManager.playerStats.TakeStaminaDamage(Mathf.RoundToInt(weaponSlotManager.attackingWeapon.baseStamina * weaponSlotManager.attackingWeapon.heavyAttackMultiplier));
    }

    public void StopWeaponFX()
    {
        weaponSlotManager.playerEffectsManager.StopWeaponFX(false);
    }

    public void SetRightWeaponTags(string[] tags)
    {
        weaponSlotManager.rightDamageCollider.enemyTags.AddRange(tags);
    }

    public void SetLeftWeaponTags(string[] tags)
    {
        weaponSlotManager.leftDamageCollider.enemyTags.AddRange(tags);
    }

    public void UsePotion()
    {
        playerInventory.UsePotion();
        playerAudio.Potion();
    }

    public void EnablePotion()
    {
        potionBottle.SetActive(true);
    }

    public void DisablePotion()
    {
        potionBottle.SetActive(false);
    }

    public void DisableStaminaRegen()
    {
        if (playerStats.canStaminaRegen)
        {
            playerStats.DisableStaminaRegen();
        }
    }

    public void DrainStaminaRoll()
    {
        weaponSlotManager.playerStats.TakeStaminaDamage((int)weaponSlotManager.playerStats.rollStaminaCost);
    }

    public void DrainStaminaSprint()
    {
        weaponSlotManager.playerStats.TakeStaminaDamage((int)weaponSlotManager.playerStats.sprintStaminaCost);
    }

    public void DrainStaminaJump()
    {
        weaponSlotManager.playerStats.TakeStaminaDamage((int)weaponSlotManager.playerStats.jumpStaminaCost);
    }

    public void Footstep(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5)
        {
            playerAudio.Footstep();
        }
    }

    public void Death()
    {
        playerAudio.Death();
    }

    public void SwordSwing()
    {
        playerAudio.SwordSwing();
    }

    public void BodyDrop()
    {
        playerAudio.BodyDrop();
    }

    public void Defeat()
    {
        playerAudio.Defeat();
    }

    public void Pickup()
    {
        playerAudio.Pickup();
    }

    //public void Roll()
    //{
    //    playerAudio.roll();
    //}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;
    public bool rightHand;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        Debug.Log("WeaponPickup");
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        if (rightHand)
        {
            playerInventory.rightHandWeapons.Add(weapon);
            playerInventory.EquipNewWeapon();
        }
        else
        {
            playerInventory.weaponInLeftHandSLots[0] = weapon;
        }
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemPopUpGameObject.GetComponentInChildren<TMP_Text>().text = weapon.name + " (F)";
        playerManager.itemPopUpGameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().enabled = true;
        playerManager.itemPopUpGameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = weapon.itemIcon;
        playerManager.itemPopUpGameObject.SetActive(true);
        Destroy(gameObject);
    }
}

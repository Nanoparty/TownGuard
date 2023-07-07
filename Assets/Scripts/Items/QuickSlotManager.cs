using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotManager : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;
    public Image potionIcon;
    public TMP_Text potionText;

    public void UpdateWeaponQuickSlots(bool isLeft, WeaponItem weapon)
    {
        //Debug.Log("SET ICON");
        if (isLeft)
        {
            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
            
        }
        else
        {
            //Debug.Log("Updating Right Quick Slot");
            if (weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
    }

    public void UpdatePotions(int count)
    {
        potionText.text = count.ToString();
    }
}

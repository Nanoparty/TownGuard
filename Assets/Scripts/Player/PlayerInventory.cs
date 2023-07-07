using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;
    QuickSlotManager quickSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    public WeaponItem unarmedWeapon;

    public List<WeaponItem> rightHandWeapons = new List<WeaponItem>();
    public WeaponItem[] weaponInLeftHandSLots = new WeaponItem[2];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = -1;

    public List<WeaponItem> weaponsInventory;

    public int potionCount = 3;

    private void Awake()
    {
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
        quickSlotManager = FindObjectOfType<QuickSlotManager>();
    }

    private void Start()
    {
        //rightWeapon = unarmedWeapon;
        //leftWeapon = unarmedWeapon;

        //weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false, new string[1] { "Enemy" });
        //weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true, new string[1] { "Enemy" });

        rightHandWeapons.Add(unarmedWeapon);
        //EquipNewWeapon();
        ChangeRightWeapon(new string[1] { "Enemy" });
    }

    public void LoadNewWeapon()
    {
        weaponSlotManager.LoadNewestWeapon(new string[1] { "Enemy" });
    }

    public void EquipNewWeapon()
    {
        currentRightWeaponIndex = rightHandWeapons.Count - 1;

        if (rightHandWeapons[currentRightWeaponIndex] != null)
        {
            rightWeapon = rightHandWeapons[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false, new string[1] { "Enemy" });
        }

    }

    public void UsePotion()
    {
        if (potionCount > 0)
        {
            potionCount--;
            playerStats.Heal(100);
            quickSlotManager.UpdatePotions(potionCount);
        }
    }

    public void SetPotionCount(int i)
    {
        potionCount = i;
        quickSlotManager.UpdatePotions(potionCount);
    }

    public void ChangeRightWeapon(string[] tags)
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > rightHandWeapons.Count - 1)
        {
            currentRightWeaponIndex = 0;
        }

        if (rightHandWeapons[currentRightWeaponIndex] != null)
        {
            rightWeapon = rightHandWeapons[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false, tags);
        }
        
    }

    public void ChangeLeftWeapon(string[] tags)
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponInLeftHandSLots[0] != null)
        {
            leftWeapon = weaponInLeftHandSLots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true, tags);
        }
        else if (currentLeftWeaponIndex == 0 && weaponInLeftHandSLots[0] == null)
        {
            currentLeftWeaponIndex++;
        }
        else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSLots[1] != null)
        {
            leftWeapon = weaponInLeftHandSLots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true, tags);
        }
        else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSLots[1] == null)
        {
            currentLeftWeaponIndex++;
        }

        if (currentLeftWeaponIndex > weaponInLeftHandSLots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true, tags);
        }
    }
}

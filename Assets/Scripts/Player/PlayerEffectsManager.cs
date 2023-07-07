using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    public GameObject currentParticleFx;
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    public void PlayWeaponFX(bool isLeft)
    {
        if (isLeft)
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
    }

    public void StopWeaponFX(bool isLeft)
    {
        //rightWeaponFX.StopWeaponFX();
    }
}

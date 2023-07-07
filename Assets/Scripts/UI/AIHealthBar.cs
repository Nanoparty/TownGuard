using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealthBar : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void SetHealth(float hp, float maxHp)
    {
        float hpPercent = hp / maxHp;
        slider.value = hpPercent;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager: MonoBehaviour
{

    public Slider slider;
    public Text hpText;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        hpText.text=(maxHealth.ToString()+"/"+maxHealth.ToString());

    }

    public void SetHealth(int health)
    {
        slider.value = health;
        hpText.text = (health.ToString() + "/" + slider.maxValue.ToString());
    }
}

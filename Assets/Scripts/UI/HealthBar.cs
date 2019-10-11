using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthbar;

    /// <summary>
    /// Set's the healthbar percentage
    /// </summary>
    /// <param name="currentPercent">the current health / max health</param>
    public void setHealth(float currentPercent)
    {
        healthbar.value = currentPercent;
    }

    
}

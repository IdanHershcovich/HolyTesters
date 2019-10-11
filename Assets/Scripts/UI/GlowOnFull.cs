using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowOnFull : MonoBehaviour
{
    public Slider super;
    public GameObject glowParticles;

    public void OnFull(float f)
    {
        if(super.value == super.maxValue)
        {
            glowParticles.SetActive(true);
        }
        else
        {
            glowParticles.SetActive(false);
        }
    }
}

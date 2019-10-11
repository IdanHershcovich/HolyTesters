using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Grail : Grail
{
    void Start()
    {
        grailName = "Fire_Grail";
        baseDamage = 2.0f;
        superDamage = 10.0f;
        superBarPercentage = 0.0f;
    }

    public override void baseEffect()
    {
        //Debug.Log("Fire Grail Base Effect Activated");
    }
    public override void superEffect()
    {
        //Debug.Log("Fire Grail Super Effect Activated");
    }
}

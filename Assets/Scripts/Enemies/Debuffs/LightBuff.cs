using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBuff : Debuff
{
    public LightBuff(float dur) : base(dur){
        stackable = false;
    }

    public override void Apply(){}
    public override void UnApply(){}
}

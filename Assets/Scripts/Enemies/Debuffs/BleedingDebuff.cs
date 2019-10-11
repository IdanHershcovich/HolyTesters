using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingDebuff : Debuff
{
    private float tickRate = 1f;
    private int tickCount = 0;
    private float dmg = 0f;
       
    public BleedingDebuff(float dur) : base(dur){
        stackable = false;
        //renderer = tar.gameObject.GetComponent<Renderer>();
    }


    public override void Apply(){
        if(dmg != 0f && tickRate * tickCount < durationTimer){
            
            target.TakeDebuffDamage(dmg);
            tickCount +=1;
        }
        target.Tint(Color.red,1f);
        
    }

    public override void UnApply(){
        target.UnTint();
    }
}

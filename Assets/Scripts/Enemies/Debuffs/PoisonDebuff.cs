using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    public PoisonDebuff(float dur) : base(dur){}
    // THIS SHITS BROKE HEH
    public override void Apply(){}
    public override void UnApply(){}

    /*
    // Only works for enemies right now

    private readonly float damageRate = 1.0f;
    private readonly int damage = 1;
    private int damageCount = 0;
    private Renderer renderer;
    public PoisonDebuff(float dur) : base(dur){
        //renderer = tar.gameObject.GetComponent<Renderer>();
    }
    
    public override void Apply(){
        if( (damageCount+1)*damageRate < durationTimer){
            damageCount++;
            target.health -= damage;
        }
        //renderer.material.SetColor("_Color",Color.green);
    }

    public override void UnApply(){
        //renderer.material.SetColor("_Color",Color.white);
    }*/
}

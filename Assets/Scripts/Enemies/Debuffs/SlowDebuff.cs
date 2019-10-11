using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : Debuff
{
    private Renderer renderer;
    public const float startingSlow = .25f;
    public float maxSlow = .70f;
    public float slowAmount = 0f;
    public float decayRate = 20f;
    

    // Only works for enemies right now
    public SlowDebuff(float dur) : base(dur){
        stackable = true;
    }

    public override void Apply(){
        target.SetMoveSpeedMultiplier(1f - slowAmount);
        target.Tint(Color.blue,1f);
    }

    public override void UnApply(){
        target.SetMoveSpeedMultiplier(1f);
        target.UnTint();
    }

    protected override void AddStack(){
        if(slowAmount + startingSlow > maxSlow){
            slowAmount = maxSlow;
        }
        else{
            slowAmount += startingSlow;
        }
    }

    protected override void Decay(float dt){
        slowAmount -= decayRate * (dt* (durationTimer/duration) * slowAmount);
    }
    


}

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public abstract class Debuff
{
    protected readonly float duration;
    public bool isExpired = false;
    public float durationTimer = 0f;
    protected Buffable target;
    public bool stackable = false;
    

    public Debuff(float dur){
        duration = dur;
    }

    public abstract void Apply();
    public abstract void UnApply();
    
    protected virtual void Decay(float dt){}

    // this is called by buffables, it doesnt just call on its own every frame
    public void Update()
    {
        if(durationTimer>duration){
            Deregister();
        }
        else{
            Apply();
            Decay(Time.deltaTime);
        }
        durationTimer += Time.deltaTime;
    }

    private void Deregister(){
        UnApply();
        isExpired = true;
    }

    public void SetTarget(Buffable e){
        target = e;
    }

    public bool Equals (Debuff obj){
        return this.GetType().Equals(obj.GetType());
    }

    public void Stack(Debuff db){
        durationTimer = db.duration;
        AddStack();
    }

    protected virtual void AddStack(){}
}

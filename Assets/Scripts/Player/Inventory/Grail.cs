using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Grail : MonoBehaviour
{
    public string grailName;

    //Base effect variables
    public float baseDamage;
    public float baseKnockback;
    public float baseStunTime;
    public ParticleSystem[] baseParticles;
    public float baseAttackLength; //time that the attack is active;
    public float baseCoolDown; //time between consecutive attacks, put 0 for continous 
    public bool baseIsProjectile;
    public bool megaPlayed;

    //Super effect variables
    public float superDamage;
    public float superKnockback;
    public float superStunTime;
    public ParticleSystem[] superParticles;
    public AnimationTransition.SuperAnimationType superAnimation;
    public float superBarPercentage;
    public float maxBarPercent = 100.0f;
    public float superChargeSpeed = .01f;
    public float superAttackLength; //time that the attack is active;
    public float superEndLag; //end lag after super

    public virtual string getName() {
        return grailName;
    }
    public virtual void setName(string name) {
        grailName = name;
    }

    public abstract void baseEffect();
    public abstract void superEffect();
    

    public float getSuperBarPercent() {
        return superBarPercentage;
    }

    public void resetSuperBar() {   
        superBarPercentage = 0.0f;
        megaPlayed = false;
    }
    //Adds amount given from parameter and limits it to maxBarPercent;
    public void addSuperBarPercent(float percentage = 0) {
        if (percentage == 0)
            superBarPercentage += superChargeSpeed;
        else 
            superBarPercentage += percentage;
            
        if (superBarPercentage > maxBarPercent) {
            if (!megaPlayed)
            {
                SoundController.Instance.PlaySoundEffect(SoundType.MEGA_READY);
            }
            megaPlayed = true;
            superBarPercentage = maxBarPercent;
        }
    }

    // used to slow player after using an attack
    protected void SlowPlayer(float duration, float slowAmount){
        SlowDebuff db = new SlowDebuff(duration);
        db.decayRate = 0;
        db.slowAmount = slowAmount;
        db.maxSlow = slowAmount;
        Player.Instance.RegisterDebuff(db);
    }
    
}

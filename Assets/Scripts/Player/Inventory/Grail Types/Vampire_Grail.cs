using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Grail : Grail
{
    [SerializeField]
    private GameObject hitBoxPrefab;

    [SerializeField]
    private GameObject superBoltPrefab;

    [SerializeField]
    private float corruptionScale;

    [SerializeField]
    private float baseHealthCost;
    void Start()
    {
        grailName = "Vampire_Grail";
        superBarPercentage = 0.0f;
    }

    public override void baseEffect() {
        DamagePlayer(baseHealthCost);
        SoundController.Instance.PlaySoundEffect(SoundType.VAMPIRE_BASIC);
        VampireGrailBase b = Instantiate(hitBoxPrefab, Player.Instance.transform.position, Player.Instance.transform.rotation*Quaternion.Euler(0,90f,0),Player.Instance.gameObject.transform).GetComponent<VampireGrailBase>();
        b.SetValues(baseDamage * getCorruption(), baseKnockback, baseStunTime);
    }

    private void DamagePlayer(float f){
        return;
        if(f < Player.Instance.GetCurrentHealth() && Player.Instance.GetPercentHealth() > .2f){
            Player.Instance.TakeDebuffDamage(f);
        }
    }

    private float getCorruption(){
        return 1f;
        float percentHealth = Player.Instance.GetPercentHealth();
        float percentEmpty = 1f-percentHealth; // ranges from 0 at full health to 1 at 0 health
        Debug.Log(percentEmpty);
        return( 1f + (corruptionScale * percentEmpty));
    }

    public override void superEffect() {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            Enemy e = g.GetComponent<Enemy>();
            if(e.HasDebuff(new BleedingDebuff(2f))){
                Succ(e);
            }            
        }
    }

    // s u c c
    private void Succ(Enemy e){
        this.resetSuperBar();
        e.RemoveDebuff(new BleedingDebuff(2f));
        SoundController.Instance.PlaySoundEffect(SoundType.VAMPIRE_MEGA);
        e.ReceiveAttack(new HitAttr(superDamage,0f));
        Instantiate(superBoltPrefab, e.gameObject.transform.position,Quaternion.Euler(0,0,0)).GetComponent<VampireGrailBase>();
    }
}

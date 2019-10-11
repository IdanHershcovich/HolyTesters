using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Grail : Grail
{
    
    [SerializeField] 
    private GameObject baseProjectile;
    [SerializeField]
    private GameObject superPrefab;
    
    private float attackSpeed = .3f;

    void Start()
    {
        
    }

    public override void baseEffect()
    {
        SoundController.Instance.PlaySoundEffect(SoundType.LIGHT_BASIC);
        LightBasicProjectile forward = Instantiate(baseProjectile,(Player.Instance.transform.position + Player.Instance.transform.forward),Player.Instance.transform.rotation * Quaternion.Euler(0, 90, 0),Player.Instance.transform).GetComponent<LightBasicProjectile>();        
        forward.SetValues(baseDamage, baseKnockback, baseStunTime, baseAttackLength, attackSpeed);
    }
    public override void superEffect()
    {
        SoundController.Instance.PlaySoundEffect(SoundType.LIGHT_HEAVY);
        this.resetSuperBar();
        Instantiate(superPrefab,Player.Instance.transform.position,Player.Instance.transform.rotation).GetComponent<LightSuperCircle>();        
    }
}

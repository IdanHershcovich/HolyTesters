using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Grail : Grail
{ 
    /// <summary> The reference to the prefab for the projectile </summary>
    [SerializeField] private GameObject projectile;
    
    /// <summary> The reference to the basic attack object </summary>
    [SerializeField]
    private GameObject objectPrefab;

    private float attackSpeed = .25f;

    void Start() {
        grailName = "Ice_Grail";
        superChargeSpeed = 0.025f;
        baseDamage = 16f;
        //baseCoolDown = .25f;
        //superEndLag = .25f;
    }

    public override void baseEffect()
    {
        SlowPlayer(.2f,.9f);
        SoundController.Instance.PlaySoundEffect(SoundType.ICE_MEGA);
        IceSuperProjectile forward = Instantiate(projectile,Player.Instance.transform.position,Player.Instance.transform.rotation * Quaternion.Euler(0, 90, 0)).GetComponent<IceSuperProjectile>();        
        forward.attackPower = 40;
        forward.speed = 50;
        forward.direction = Player.Instance.transform.forward;
        
        
    }

    public override void superEffect() {
        this.resetSuperBar();
        IceGrailBasicAttack currentObject
            = Instantiate(objectPrefab, Player.Instance.transform.position, Player.Instance.transform.rotation, Player.Instance.transform).GetComponent<IceGrailBasicAttack>();

        currentObject.SetValues(superDamage, superKnockback, superStunTime, superAttackLength, attackSpeed);

    }
}

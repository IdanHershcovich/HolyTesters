using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_Grail : Grail
{
    /// <summary> The reference to the prefab for the projectile </summary>
    [SerializeField] private GameObject projectile;
    
    /// <summary> The reference to the basic attack object </summary>
    //[SerializeField]
    //private GameObject objectPrefab;

    private float attackSpeed = .25f;
    private bool clockwise = true;

    void Start() {
        grailName = "Mech_Grail";
        //superChargeSpeed = .5f;
        //baseCoolDown = .25f;
        //superEndLag = .25f;
    }

    public override void baseEffect()
    {
        SlowPlayer(.2f,.9f);
        Fire(0, baseDamage);
        Fire(15f, baseDamage);
        Fire(-15f, baseDamage);
        
    }

    private void Fire(float deg, float dmg){
        SoundController.Instance.PlaySoundEffect(SoundType.MECH_BASIC);
        GameObject gobject = Instantiate(projectile, Player.Instance.transform.position, Player.Instance.transform.rotation*Quaternion.Euler(0,deg,0));
        MechBasicProjectile mechProjectile = gobject.GetComponent<MechBasicProjectile>();
        if(!clockwise){
            gobject.GetComponent<RotateMe>().direction = -1;
        }
        clockwise = !clockwise;
        mechProjectile.direction = gameObject.transform.forward;
        mechProjectile.target = Player.Instance.transform.position + (Quaternion.Euler(0,deg,0)*Player.Instance.transform.forward) * 10f;
        mechProjectile.attackPower = dmg;
    }

    public override void superEffect() {
        SlowPlayer(.2f,.9f);
        this.resetSuperBar();
        SoundController.Instance.PlaySoundEffect(SoundType.MECH_MEGA);
        for (int i = 0; i < 24; i++){
            Fire((float)i*15, superDamage);
        }
    }
}

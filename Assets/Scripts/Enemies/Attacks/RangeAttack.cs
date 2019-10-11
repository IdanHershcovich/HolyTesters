using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attack
{
    #region fields

    //GameObject to be instantiated, PlayerStatus to augment health eventually
    [SerializeField]
    protected GameObject projectile;

    /// <summary>
    /// Effects of projectile
    /// </summary>
    //[SerializeField]
    //protected HitAttr effects;
    //we are getting these properties from the abstract class Attack.

    #endregion

    private EnemyProjectileScript forward;

    public float DelayBetweenAttacks;

    //Override abstract function in Attack
    public override void Execute(Enemy enemy, Transform target)
    {

        Animator animator = enemy.GetAnimator();
        animator.SetTrigger("Attack");

        ////Debug.Log("RangeAttack.Execute()");
        //SynchedTripleShot(target);
        StraightThenSides(target);
    }
    //Method in abstract class, meaning it must be here.
    public override bool InRangeOf(Transform target)
    {
        // TODO: use a raycast to see if shot will line up with player
        return true;
    }
    //spawns a projectile and sends it forward
    public void StraightShot(Transform target)
    {
        SoundController.Instance.PlaySoundEffect(SoundType.BAT_SHOT);
        projectile = ObjectPooler.SharedInstance.GetPooledObject("Projectile");
        forward = projectile.GetComponent<EnemyProjectileScript>();//Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        projectile.SetActive(true);
        forward.timer = 0.0f;
        forward.transform.position = transform.position;
        //forward.effects = effects;
        forward.direction = Vector3.ProjectOnPlane((target.transform.position - transform.position).normalized, Vector3.up);
    }
    //spawns a projectile and sends it off-center to the left
    public void LeftShot()
    {
        SoundController.Instance.PlaySoundEffect(SoundType.BAT_SHOT);
        projectile = ObjectPooler.SharedInstance.GetPooledObject("Projectile");
        EnemyProjectileScript skewedLeft = projectile.GetComponent<EnemyProjectileScript>();//Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        projectile.SetActive(true);
        skewedLeft.transform.position = transform.position;
        //skewedLeft.effects = effects;
        skewedLeft.direction = Quaternion.Euler(0, -15, 0) * forward.direction;
    }
    //spawns a projectile and sends it off-center to the right
    public void RightShot()
    {
        SoundController.Instance.PlaySoundEffect(SoundType.BAT_SHOT);
        projectile = ObjectPooler.SharedInstance.GetPooledObject("Projectile");
        EnemyProjectileScript skewedRight = projectile.GetComponent<EnemyProjectileScript>();//Instantiate(projectile).GetComponent<EnemyProjectileScript>();
        projectile.SetActive(true);
        skewedRight.transform.position = transform.position;
        //skewedRight.effects = effects;
        skewedRight.direction = Quaternion.Euler(0, 15, 0) * forward.direction;
    }
    //Shoot all three projecticles simultaneously. 
    public void SynchedTripleShot(Transform target)
    {
        StraightShot(target);
        LeftShot();
        RightShot();
    }
    //Shoot the straight projectile first, then after a delay shoot the side projectiles.
    public void StraightThenSides(Transform target)
    {
        StartCoroutine(straightThenSides(DelayBetweenAttacks, target));

    }
    //Coroutine for the delay of the above func.
    private IEnumerator straightThenSides(float seconds, Transform target)
    {
        StraightShot(target);
        yield return new WaitForSeconds(seconds);
        LeftShot();
        RightShot();
    }
    /*
    //TODO, sidesThenStraight
    private IEnumerator sidesThenStraight(float seconds, Transform target)
    {

    }
    */
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behaviour of an enemy projectile
/// </summary>
public class EnemyProjectileScript : Attack
{
    /// <summary> Direction of movement </summary>
    public Vector3 direction; //unneeded, while go back and change later.

    //The enemy that is shooting, so we can get damage values, knockback values et al.
    //This will allow multiple enemies (potentially) derive from this class, while retaining individual characteristics on a per-enemy basis.
    public Enemy shooter;

    /// <summary> Timer for how long projectile has been active </summary>
    public float timer;

    /// <summary> Max time before projectile is destroyed </summary>
    private float maxTime = 5;

    private void Start()
    {
        //Inherent attributes from the RangeAttack Component on individual enemies, which will allow
        //projectiles to have different attributes depending on the Enemy shooting rather than hardcoding
        //attributes to different projectiles. 
        /*
        RangeAttack ra = shooter.GetComponent<RangeAttack>();
        damage = ra.damage;
        knockBackSpeed = ra.knockBackSpeed;
        stunTime = ra.stunTime;
        speed = ra.speed;
        */
    }

    void OnTriggerEnter(Collider col)
    {
        //if player is hit, damage player
        if (col.CompareTag("PlayerRangedHitBox"))
        {
            ////Debug.Log("Should do damage to Player now");
            ApplyEffectsToPlayer(shooter, Player.Instance);
        }

        //if projectile hits anything, destroy it
        //Destroy(gameObject);
        gameObject.SetActive(false);
        ////Debug.Log("Why we trigger here?");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move projectile by direction and speed
        transform.position += direction * speed * Time.fixedDeltaTime;

        //increment timer
        timer += Time.fixedDeltaTime;

        //if timer exceeds max time, destroy object
        if (timer >= maxTime)
            gameObject.SetActive(false);
        //Destroy(gameObject);

    }

    //Override abstract function in Attack, leave blank for now
    public override void Execute(Enemy enemy, Transform target)
    {
        ////Debug.Log("RangeAttack.Execute()");
        //SynchedTripleShot(target);
        //StraightThenSides(target);
    }
    //Method in abstract class, meaning it must be here, even though for range its fairly pointless.
    public override bool InRangeOf(Transform target)
    {
        // TODO: use a raycast to see if shot will line up with player
        return true;
    }
}

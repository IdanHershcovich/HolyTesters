using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSuperProjectile : MonoBehaviour
{
    /// <summary> Direction of movement </summary>
    public Vector3 direction;
    /// <summary> speed of movement </summary>
    public float speed;


    /// <summary> power of the projectile attack </summary>
    public float attackPower;
    /// <summary> stun time of the projectile attack </summary>
    public float attackStun;
    /// <summary> knockback strength of the projectile attack </summary>
    public float attackKnockback;
    /// <summary> debuff of the projectile attack </summary>
    public Debuff attackDebuff;

    /// <summary> Timer for how long projectile has been active </summary>
    private float timer;
    /// <summary> Max time before projectile is destroyed </summary>
    private float maxTime = 5;

    void OnTriggerEnter(Collider col)
    {
        //if player is hit, damage player
        if (col.gameObject.tag == "Enemy")
        {
            Enemy e = col.gameObject.GetComponent<Enemy>();
            e.RegisterDebuff(new SlowDebuff(10f));
            e.ReceiveAttack(new HitAttr(attackPower, attackKnockback * direction, attackStun));
        }
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
            Destroy(gameObject);

    }
}

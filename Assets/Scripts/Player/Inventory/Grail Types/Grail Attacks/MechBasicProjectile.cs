using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MechBasicProjectile : MonoBehaviour
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
    private float maxTime = 2f;
    private float phaseOneLength = 1f;
    private bool returning = false;
    public Vector3 target;

    private List<Enemy> enemiesHit = new List<Enemy>();

    void OnTriggerEnter(Collider col)
    {
        Enemy e = col.gameObject.GetComponent<Enemy>();// potential error if projetile collides with non enemy
        //if player is hit, damage player
        if (col.gameObject.tag == "Enemy" && ! enemiesHit.Any(x=> x.serialNo == e.serialNo))
        {
            
            e.ReceiveAttack(new HitAttr(attackPower, attackKnockback * direction, attackStun,0f));
            enemiesHit.Add(e);
        }
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if((target - gameObject.transform.position).magnitude <1f && !returning){
            returning = true;
            enemiesHit.RemoveAll(x=>true);
        }
        else if(returning){
            target = Player.Instance.transform.position;    
        }
        
        
        //Move projectile by direction and speed
        transform.position += (target - transform.position) * speed * Time.fixedDeltaTime;

        

        //increment timer
        timer += Time.fixedDeltaTime;

        //if timer exceeds max time, destroy object
        if (timer >= maxTime || (timer > phaseOneLength && (Player.Instance.transform.position - gameObject.transform.position).magnitude < 1.5f))
            Destroy(gameObject);

    }

        // Update is called once per frame
    /*void FixedUpdate()
    {
        if((target - gameObject.transform.position).magnitude <.5f && !phaseOne){
            target = Player.Instance.transform.position;    
        }
        
        
        //Move projectile by direction and speed
        transform.position += (target - transform.position) * speed * Time.fixedDeltaTime;
        

        //increment timer
        timer += Time.fixedDeltaTime;

        //if timer exceeds max time, destroy object
        if (timer >= maxTime || (!phaseOne && (Player.Instance.transform.position - gameObject.transform.position).magnitude < 1.5f))
            Destroy(gameObject);

    }*/
}

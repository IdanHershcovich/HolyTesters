using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VampireGrailBase : MonoBehaviour
{
    private float damage;
    private float knockback;
    private float stunTime;
    private float timer = 0f;
    private float maxTime = .5f;
    private List<Enemy> hitEnemies;
    [SerializeField]
    private List<VampireSlash> vampireSlashes;

    void Start(){
        hitEnemies = new List<Enemy>();
        foreach (VampireSlash vs in vampireSlashes)
            vs.Slash();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //increment timer
        timer += Time.fixedDeltaTime;

        //if timer exceeds max time, destroy object
        if (timer >= maxTime)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    public void SetValues(float dmg, float kb, float stun)
    {
        damage = dmg;
        knockback = kb;
        stunTime = stun;
    }
 
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Enemy" )
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(!hitEnemies.Any(x => x==enemy)){
                hitEnemies.Add(enemy);
                Vector3 kb = knockback * Vector3.ProjectOnPlane(enemy.transform.position - Player.Instance.transform.position, Vector3.up).normalized;
                HitAttr hitAttr = new HitAttr(damage, kb, stunTime);
                enemy.ReceiveAttack(hitAttr);
                enemy.RegisterDebuff(new BleedingDebuff(12f));
            }
        }
    }

}

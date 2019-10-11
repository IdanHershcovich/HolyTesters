using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGrailBasicAttack : MonoBehaviour
{

    private float damage;
    private float knockback;
    private float stunTime;
    private float attackLength;
    private float attackSpeed;
    private List <GameObject> currentCollisions = new List <GameObject> ();

    private void Start()
    {
        StartCoroutine(baseEffectCoroutine());
    }

    // Start is called before the first frame update
    public void SetValues(float dmg, float kb, float stun, float length, float atkspd)
    {
        damage = dmg;
        knockback = kb;
        stunTime = stun;
        attackLength = length;
        attackSpeed = atkspd;
    }

    public IEnumerator baseEffectCoroutine()
    {
        int attackCount = 0;
        for (float timer = 0; timer < attackLength; timer += Time.deltaTime)
        {
            if(attackCount * attackSpeed < timer){
                DealDamage();
                attackCount++;
            }
            yield return null;
        }
        Destroy(gameObject, Time.fixedDeltaTime);
    }

    private void DealDamage(){
        foreach(GameObject g in currentCollisions){
            try
            {
                Enemy enemy = g.GetComponent<Enemy>();
                Vector3 kb = knockback * Vector3.ProjectOnPlane(enemy.transform.position - Player.Instance.transform.position, Vector3.up).normalized;
                HitAttr hitAttr = new HitAttr(damage, kb, stunTime,0f);

                enemy.ReceiveAttack(hitAttr);
                enemy.RegisterDebuff(new SlowDebuff(10f));
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
     
    private void OnTriggerEnter (Collider other) {
        // Add the GameObject collided with to the list.
        if (other.gameObject.tag == "Enemy")
        {
            currentCollisions.Add (other.gameObject);
        }
    }
 
    private void OnTriggerExit (Collider other) {
        try
        {
            if (other.gameObject.tag == "Enemy")
            {
                currentCollisions.Remove (other.gameObject);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Warning: IceGrailBasicAttack error 2");
        }
    }
}

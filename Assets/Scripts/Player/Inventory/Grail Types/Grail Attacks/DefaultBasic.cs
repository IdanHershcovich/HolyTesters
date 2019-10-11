using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBasic : MonoBehaviour
{
    private float damage;
    private float knockback;
    private float stunTime;

    // Start is called before the first frame update
    public void SetValues(float dmg, float kb, float stun)
    {
        damage = dmg;
        knockback = kb;
        stunTime = stun;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Vector3 kb = knockback * Vector3.ProjectOnPlane(enemy.transform.position - Player.Instance.transform.position, Vector3.up).normalized;
            HitAttr hitAttr = new HitAttr(damage, kb, stunTime);

            enemy.ReceiveAttack(hitAttr);
        }
    }
}

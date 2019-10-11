using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBasicProjectile : MonoBehaviour
{
    private float timer = 0f;
    private int attackCount = 0;
    private const float mintime = 0;
    private List <GameObject> currentCollisions = new List <GameObject> ();
    private Rewired.Player rwplayer;
    // for checking if player has light buff
    private LightBuff lbref;
    
    private float damage;
    private float knockback;
    private float stunTime;
    private float attackLength;
    private float attackSpeed;
    
    // grow the transforms xyz values
    private float fullx;
    private float fully;
    private float fullz;
    private float baseChargeCoeff = .2f;
    private float superChargeCoeff = 1f;
    private Vector3 startingScale;
    private float charge = 0;
    // Max charge value while not in super
    private float noSuperMaxCharge =.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        lbref = new LightBuff(200f);
        startingScale = gameObject.transform.localScale;
        fullx = 2f;
        fully = 6f;
        fullz = 6f;
        rwplayer = Rewired.ReInput.players.GetPlayer(0);
    }
    
    public void SetValues(float dmg, float kb, float stun, float length, float atkspd)
    {
        damage = dmg;
        knockback = kb;
        stunTime = stun;
        attackLength = length;
        attackSpeed = atkspd;
    }

    private void Grow(){
        gameObject.transform.localScale = new Vector3(
            startingScale.x + charge * (fullx - startingScale.x),
            startingScale.y + charge * (fully - startingScale.y),
            startingScale.z + charge * (fullz - startingScale.z));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        SlowPlayer(.1f, .95f);
        if(Player.Instance.HasDebuff(lbref)){
            IncrementCharge(superChargeCoeff * Time.deltaTime);
        }
        else if(charge > noSuperMaxCharge){
            IncrementCharge(baseChargeCoeff * -1*Time.deltaTime);
        }
        else{
            IncrementCharge(baseChargeCoeff * Time.deltaTime,noSuperMaxCharge);
        }
        Grow();

        
        
        if(attackCount * attackSpeed < timer){
            DealDamage();
            attackCount++;
        }

        
        if(!rwplayer.GetButton("Fire1") && timer>mintime){
            //SlowPlayer(0f, 0f);
            Destroy(gameObject);
        }
    }

    private void IncrementCharge(float f, float max = 1f){
        charge += f;
        if(charge>max){
            charge = max;
        }
        else if (charge < 0){
            charge = 0f;
        }
    }

    private void SlowPlayer(float duration, float slowAmount){
        SlowDebuff db = new SlowDebuff(duration);
        db.decayRate = 0;
        db.slowAmount = slowAmount;
        db.maxSlow = slowAmount;
        Player.Instance.RegisterDebuff(db);
    }

    private void DealDamage(){
        Debug.Log(currentCollisions.Count);
        foreach(GameObject g in currentCollisions){
            try
            {
                Enemy enemy = g.GetComponent<Enemy>();
                Vector3 kb = knockback * Vector3.ProjectOnPlane(enemy.transform.position - Player.Instance.transform.position, Vector3.up).normalized;

                HitAttr hitAttr = new HitAttr(damage * (charge+1), kb, stunTime,0f);

                enemy.ReceiveAttack(hitAttr);
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
            Debug.Log("Warning: LightBasicAttack error 2");
        }
    }
    
}

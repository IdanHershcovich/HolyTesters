using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Electric_Grail : Grail
{
    [SerializeField]
    private GameObject zapPrefab;

    private List<Lightning> zapList;
    // Start is called before the first frame update
    private float superRateOfFire = .15f;
    private float superNumberOfAttacks = 15;
    void Start()
    {
        zapList = new List<Lightning>();
        for(int i = 0; i < 20; i ++){
            GameObject temp = Instantiate(zapPrefab);
            DontDestroyOnLoad(temp);
            zapList.Add(temp.GetComponent<Lightning>());
        }
    }

    public override void baseEffect(){
        SoundController.Instance.PlaySoundEffect(SoundType.ELECTRIC_BASIC);
        Shoot(baseDamage);
    }
    public override void superEffect(){
        SlowDebuff db = new SlowDebuff(superRateOfFire*superNumberOfAttacks);
        db.decayRate = 0f;
        db.slowAmount = .4f;
        db.maxSlow = .4f;
        Player.Instance.RegisterDebuff(db);
        SuperShoot();
        SoundController.Instance.PlaySoundEffect(SoundType.ELECTRIC_MEGA);
        InvokeRepeating("SuperShoot",0f,superRateOfFire);
        Invoke("StopSuper",superNumberOfAttacks*superRateOfFire);
        this.resetSuperBar();
    }
    private void StopSuper(){
        CancelInvoke();
    }

    private void SuperShoot(){
        Shoot(superDamage);
    }
    private void Shoot(float dmg ){

        Vector3 pos;
        Vector3 pos1;
        Vector3 pos2; 
        Vector3 dir = Player.Instance.gameObject.transform.forward;
        int layerMask = LayerMask.GetMask("Enemy");
        RaycastHit hit;
        for(int i = 0; i < 20; i +=1){
            pos = Player.Instance.gameObject.transform.position;
            pos1 = Quaternion.Euler(0, 1*i, 0) * pos;
            pos2 =  Quaternion.Euler(0, -1*i, 0) * pos;
            dir = Player.Instance.gameObject.transform.forward;
            if(Physics.Raycast(pos1,dir,out hit,100f) ||  Physics.Raycast(pos2,dir,out hit,100f)){
            try{
                if(hit.collider.gameObject.tag == "Enemy"){
                    MyZap(Player.Instance.gameObject.transform,DealDamage(dmg,hit.collider.gameObject.GetComponent<Enemy>(),2,new List<Enemy>()));
                }
                else{
                    MyZap(Player.Instance.gameObject.transform, hit.collider.gameObject.transform);
                }
                
                break;
            }
            catch(Exception e){
                Debug.Log(e);
            }
        }
        }
    }

    private Transform DealDamage(float dmg,Enemy currentTarget,int numHits,List<Enemy> HitTargets){
        HitTargets.Add(currentTarget);
        HitAttr hitAttr = new HitAttr(dmg, 0f);
        currentTarget.ReceiveAttack(hitAttr);
        if(numHits <= 0){
            return currentTarget.gameObject.transform;
        }
        else{
            Transform nextTarget = DealDamage(dmg-10f,ClosestEnemy(currentTarget,HitTargets),numHits-1,HitTargets);
            MyZap(currentTarget.gameObject.transform,nextTarget);
            return currentTarget.gameObject.transform;
        }
    }

    private void MyZap(Transform t1, Transform t2){
        zapList[0].Zap(t1,t2);
        Lightning temp = zapList[0];
        zapList.RemoveAt(0);
        zapList.Add(temp);
    }
    
    private Enemy ClosestEnemy(Enemy currentTarget,List<Enemy> excludes){
        Enemy closest = null;
        GameObject[] allEnemies = GetEnemiesInRange(currentTarget, 5f);
        foreach(GameObject g in allEnemies){
            Enemy e = g.GetComponent<Enemy>();
            if(!excludes.Any(x => x==e) && e !=currentTarget){
                if(closest == null){
                    closest = e;
                }
                else if(Vector3.Distance(currentTarget.gameObject.transform.position,e.gameObject.transform.position) <
                Vector3.Distance(currentTarget.gameObject.transform.position,closest.gameObject.transform.position))
                {
                    closest = e;
                }
            }
        }
        return closest;
    }

    private GameObject[] GetEnemiesInRange(Enemy current, float range){
        Collider[] colliders = Physics.OverlapSphere(current.gameObject.transform.position, range);
        List<GameObject> enemies = new List<GameObject>();
        enemies.Add(current.gameObject);
        foreach(Collider c in colliders){
            if(c.gameObject.tag == "Enemy"){
                enemies.Add(c.gameObject);
            }
        }
        return enemies.ToArray();
    }
    

}

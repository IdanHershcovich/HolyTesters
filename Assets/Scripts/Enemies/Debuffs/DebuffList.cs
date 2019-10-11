using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebuffList : MonoBehaviour
{
    private List<Debuff> debuffs = new List<Debuff>();

    public void ApplyDebuffEffects(){
        // can't use more complex loops because the collection modifies during enumeration
        for (int i=0; i<debuffs.Count; i++){
            debuffs[i].Update();
            if(debuffs[i].isExpired){
                DeregisterDebuff(debuffs[i]);
            }
        }
    }

    public void RegisterDebuff(Debuff db){
        Debug.Log("Registering");
        //print(debuffs.Count);
        if(HasDebuff(db) && db.stackable){
            debuffs.Find(x => x.Equals(db)).Stack(db);
            //print(((SlowDebuff)debuffs.Find(x => x.Equals(db))).slowAmount);
        }
        else{
            //print("Asdf");
            debuffs.Add(db);
        }
    }

    public void DeregisterDebuff(Debuff db){
        Debug.Log("Deregistering");
        foreach(Debuff d in debuffs.FindAll(e => e.Equals(db) && e.isExpired))
        {
            d.UnApply();
        }
        debuffs.RemoveAll(e => e.Equals(db) && e.isExpired);
        
    }
    
    public void RemoveDebuff(Debuff db){
        Debug.Log("Deregistering");
        foreach(Debuff d in debuffs.FindAll(e => e.Equals(db)))
        {
            d.UnApply();
        }
        debuffs.RemoveAll(e => e.Equals(db));
    }

    public bool HasDebuff(Debuff db){
        return debuffs.Any(x => x.Equals(db));
    }


}

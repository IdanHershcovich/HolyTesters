using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            SoundController.Instance.PlaySoundEffect(SoundType.PICKUP);
            if (!Player.Instance.inventory.addGrail(this.GetComponent<Grail>())) {
                Player.Instance.inventory.pickUpAndSwitch(this.GetComponent<Grail>(), this.gameObject.transform);
            }
            this.gameObject.transform.parent = other.gameObject.transform;
            this.gameObject.SetActive(false);
        }
        
        //Destroy(this.gameObject);
    }
        /*void OnTriggerStay() {
            //Display UI for swapping
            inventory.addGrail(this.GetComponent<Grail>());
        }*/
}

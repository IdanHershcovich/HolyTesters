using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{

    //Amount we heal player by on orb pickup
    [Range(5, 25)]
    public float healAmount;

    //Probably unneccessary, but didn't want to call the instance raw on the collision enter
    public void HealFromOrb(float healAmount)
    {
        SoundController.Instance.PlaySoundEffect(SoundType.HEAL);
        Player.Instance.GetComponent<PlayerStatus>().Heal(healAmount);
        //player.Heal(healAmount);

        
    }
    //On collision, heal and delete
    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Foo");
        //THIS IS TEMPROARY. Doing it as CompareTag("Player") is not working. This is just for this build.
        if(!collider.gameObject.CompareTag("Enemy") && !collider.gameObject.CompareTag("PlayerAttack"))
        {
            HealFromOrb(healAmount);
            //Debug.Log("Player Health Should Increase by up to 20 here");
            Destroy(gameObject);
        }
    }
}

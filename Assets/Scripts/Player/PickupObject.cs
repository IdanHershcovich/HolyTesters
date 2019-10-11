using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adds object that this script is attached to to the inventory of the player.

public class PickupObject : MonoBehaviour
{
    private Inventory inventory;


    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inventory.addItem(this.name);
        }
    }

}

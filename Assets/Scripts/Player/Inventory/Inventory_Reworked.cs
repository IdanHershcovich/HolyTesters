using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory_Reworked : Singleton<Inventory_Reworked>
{
    [SerializeField] private int maxSize = 3;
    public List<Grail> inventory;

    public Grail nullGrail;
    [SerializeField] private UI_Inventory UI_inventory;
    public float switchCooldown;
    private float nextSwitch;
    public float pickUpCooldown;
    private float nextPickUp;

    //All the grail prefabs for instantiating upon switching grail
    [SerializeField] GameObject[] grailPrefabs;

    void Start() {
        nextSwitch = Time.time;
        nextPickUp = Time.time;
        switchCooldown = UI_inventory.getSwitchSpeed();
        UI_inventory = UI_Inventory.Instance;
    }
    
    //Returns a grail object of the currently equipped grail
    public Grail getEquippedGrail() {
        return inventory[0];
    }

    public bool addGrail(Grail grail) {
        //Add the grail to inventory if inventory is not full
        foreach (Grail item in inventory)
        {
            if (item.getName() == nullGrail.getName())
            {
                int x = inventory.IndexOf(item);
                inventory[x] = grail;
                UI_inventory.updateAllSlots();
                return true;
            }
            
        }
        UI_inventory.updateAllSlots();
        return false;
    
    }
    //Activates the base effect of the equipped grail
    public void useBase()
    {
        inventory[0].baseEffect();
    }
    //Activates the super effect of the equipped grail
    public void useSuper() {
        if (inventory[0] != nullGrail)
        {
            if (inventory[0].getSuperBarPercent() == inventory[0].maxBarPercent)
            {
                inventory[0].superEffect();
            }
        }
    }
    //Destroys the grail that is passed in and instantiates the grail that is being dropped
    public void pickUpAndSwitch(Grail grailOnFloor, Transform location)
    {
        if (Time.time > nextPickUp + pickUpCooldown)
        { 
            string dropName = inventory[0].getName();
            Destroy(inventory[0].gameObject);
            inventory[0] = grailOnFloor;

            foreach (GameObject grail in grailPrefabs)
            {
                if (grail.GetComponent<Grail>().getName() == dropName)
                {
                    SoundController.Instance.PlaySoundEffect(SoundType.DROP);
                    Instantiate(grail, Player.Instance.transform.position + (transform.forward * -2), location.rotation);
                }
            }

            UI_inventory.updateAllSlots();
        }
    }
    //Calls shift right on inventory so that the new index 0 is the old index 1
    //Updates UI to match current grail
    public void swapForward() {
        if (Time.time > nextSwitch && inventory.Count >= 2)
        {
            SoundController.Instance.PlaySoundEffect(SoundType.GRAIL_SWITCH);
            UI_inventory.rotateLeft();
            inventory = inventory.ShiftRight<Grail>(1);      
            nextSwitch = Time.time + switchCooldown;
        }
        UI_inventory.updateAllSlots();
    }
    //Calls shift right on inventory so that the new index 0 is the old index 2
    //Updates UI to match current grail
    public void swapBackward() {
        if (Time.time > nextSwitch && inventory.Count >= 2)
        {
            SoundController.Instance.PlaySoundEffect(SoundType.GRAIL_SWITCH);
            UI_inventory.rotateRight();
            inventory = inventory.ShiftLeft<Grail>(1);  
            nextSwitch = Time.time + switchCooldown;
        }
        UI_inventory.updateAllSlots();

    }



    //A generic swap function
    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    
}

public static class ShiftList
{
    public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy)
    {
        if (list.Count <= shiftBy)
        {
            return list;
        }

        var result = list.GetRange(shiftBy, list.Count - shiftBy);
        result.AddRange(list.GetRange(0, shiftBy));
        return result;
    }

    public static List<T> ShiftRight<T>(this List<T> list, int shiftBy)
    {
        if (list.Count <= shiftBy)
        {
            return list;
        }

        var result = list.GetRange(list.Count - shiftBy, shiftBy);
        result.AddRange(list.GetRange(0, list.Count - shiftBy));
        return result;
    }
}

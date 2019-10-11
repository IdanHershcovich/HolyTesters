using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    /*
     Changed by Nick Tang
     5/14/19
     */

    [SerializeField] private List<string> items; //List of items as inventory
    public string equipped; //index 0 of the list

    public string getItemName(int index)
    {
        return items[index];
    }
    //Adds item to inventory
    public void addItem(string itemName)
    {
        items.Add(itemName);
        items = items.Distinct().ToList();
    }
    //Changes current equipped with itemName, if itemName is inside the list of items.
    public void changeEquip(string itemName)
    {
        if (items.Contains(itemName))
        {
            Swap<string>(items, items.IndexOf(itemName), 0);
        }
        
    }
    //Swaps the currently equipped grail with the index of: current + 1
    public string quickChangeEquip() {
        if (items.IndexOf(equipped) + 1 < items.Count) {
            SoundController.Instance.PlaySoundEffect(SoundType.GRAIL_SWITCH);
            Swap<string>(items, items.IndexOf(equipped), items.IndexOf(equipped) +1);
        }
        return items[0];
    }
    //Getter function for current equipped
    public string getEquippedName()
    {
        return equipped;
    }
    //Generic swap function for any list, swapping indexA with indexB
    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    void Start()
    {
        items = new List<string>();
    }

    void Update()
    {
        if (items.Count != 0)
        {
            equipped = items[0];
        }
    }
}


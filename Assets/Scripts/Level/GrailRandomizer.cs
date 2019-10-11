using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailRandomizer : MonoBehaviour
{
    public List<Grail> grails; //Add all grails to this list.
    public List<Grail> spawnOptions;
    public int totalRooms; 
    //public int grailUpperBound;
    public int grailLowerBound;
    public LevelGenerator LevelGenerator;
    public List<Room> rooms;
    public int totalGrails;
    public int percentOfRoomsAsUpperBound;

    private void Start()
    {
        rooms = LevelGenerator.getRooms();
        totalRooms = rooms.Count;
    }





    //Randomize which rooms get a grail
    public void RanomizeGrails()
    {
        if(grails == null || grails.Count <= 0 ){
            return;
        }
        //copy the rooms list from level builder, count the rooms for iteration purposes
        rooms = LevelGenerator.getRooms();
        totalRooms = rooms.Count;
        //make max total rooms (not including start and end rooms)
        totalGrails = Random.Range(grailLowerBound, Mathf.RoundToInt(totalRooms*percentOfRoomsAsUpperBound*.01f));
        int difference = totalRooms - totalGrails;
        //trim list down to rooms we will spawn a grail in
        for(int i = 0; i<difference; i++)
        {
            int indexToDelete = Random.Range(0, rooms.Count);
            rooms.RemoveAt(indexToDelete);
        }

        
        //For each room left in the list, spawn a randomized grail there via the room script
        foreach(Room r in rooms)
        {
            r.grailPedestal.SetActive(true);
            //Debug.Log("Hello");
            if (r.CompareTag("StartRoom"))
            {
                r.SpawnGrail();
            }
            if (r.CompareTag("EndRoom"))
            {
                r.grailPedestal.SetActive(false);
            }
            //set all grail pedestals in each room to active if it's in our list of grail rooms.
            //r.grailPedestal.SetActive(true);
        }

       


    }

    public void OrganizeSpawnableGrails()
    {
        if (Inventory_Reworked.Instance.inventory == null || Inventory_Reworked.Instance.inventory.Count <= 0)
        {
            return;
        }
        List<Grail> playerGrails = new List<Grail>(Inventory_Reworked.Instance.inventory);
        spawnOptions = new List<Grail>(grails);
        if (playerGrails == null || playerGrails.Count <= 0)
        {
            return;
        }
        for (int i = 0; i <= playerGrails.Count-1; i++)
        {
            if(spawnOptions.Contains(playerGrails[i]))
            {
                spawnOptions.Remove(playerGrails[i]);
            }
        }
    }

}

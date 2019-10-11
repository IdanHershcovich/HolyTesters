using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the barriers that pop up when player enters an uncleared room
/// clears barriers when enemies are all destroyed
/// 
/// TODO: Holy FUck rewrite this script from the ground up
/// </summary>
public class EscapePrevention : MonoBehaviour
{
    /// <summary> List of enemies in room </summary>
    public List<Enemy> enemies;
    /// <summary> set to true when player enters room </summary>
    private bool began;

    public GameObject followCam;
    //public GameObject overheadCam;

    //Rush 7/16/19
    public UnityEvent roomCleared;

    /// <summary>
    /// Room associated with this instance of EscapePrevention (assigned in Load)
    /// </summary>
    private Room room;

    public void Load(Room room, bool endRoom=false)
    {
        // Get all enemies in room
        this.room = room;
        enemies = room.Enemies;

        if(endRoom) {
            // Show end screen after clearing boss room
            roomCleared.AddListener(() => DetectBossDeath.Instance.SpawnTeleporter());
        }
        
        followCam = GameObject.FindGameObjectWithTag("Follow_Cam");
        //overheadCam = GameObject.FindGameObjectWithTag("Overhead_Cam");
    }

    /// <summary>
    /// Initializes behavior of EscapePrevention
    /// </summary>
    public void Begin()
    {
        //overheadCam.SetActive(false);
        
        //ensures room isn't empty or barriers haven't already been triggered
        if (began == true || !room.WillSpawnEnemies())
            return;

        began = true;
        
        // sets all barriers to active to block in player
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // Spawn enemies in room
        room.SpawnEnemies();
    }

    /// <summary>
    /// Removes an enemy from the list and checks to see if all have been removed
    /// </summary>
    /// <param name="enemy"> The referenced enemy to remove from the enemy list </param>
    public void RemoveEnemy(Enemy enemy)
    {
        //ensures passed enemy reference is a valid enemy in the list
        if (enemies.Contains(enemy) && enemies != null)
        {
            enemies.Remove(enemy);

            // if all enemies have bin removed, handle behavior end
            if (enemies.Count == 0)
                End();
        }
    }

    /// <summary>
    /// Ends the script behavior by dropping all the barriers
    /// </summary>
    private void End()
    {
        //overheadCam.SetActive(true);

        //Added Rush 7/16/19
        roomCleared.Invoke();

        // set all barriers to inactive
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //set ExcapePrevention object to inactive
        gameObject.SetActive(false);
    }

    IEnumerator waitingFor(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        followCam.SetActive(true);
    }
}

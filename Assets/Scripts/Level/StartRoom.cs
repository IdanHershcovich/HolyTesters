using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : Room
{
    [Header("Start Room Settings")]
    /// <summary>
    /// Position to spawn player at
    /// </summary>
    [SerializeField]
    private Transform spawnPoint;

    override public void Populate()
    {
        base.Populate();

        Debug.Log("StartRoom.Populate()");

        // Place player in spawn position
        Player.Instance.transform.position = spawnPoint.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimap Script
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    
    public int distance = 100;
  
    // Update is called once per frame
    void Update()
    {
        Transform player = Player.Instance.transform;
        gameObject.transform.position = new Vector3(player.position.x,distance, player.position.z); //make camera follow player
        gameObject.transform.eulerAngles = new Vector3(90, 45, 0);
    }
}

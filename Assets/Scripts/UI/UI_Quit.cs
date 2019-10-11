using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UI_Quit : MonoBehaviour
{
    // rewired
    private Rewired.Player rwplayer;

    public void Start(){
        rwplayer = ReInput.players.GetPlayer(0);
    }

    public void Quit() {
        Application.Quit();
    }

    void Update()
    {
        if(rwplayer.GetButtonDown("Slam")){
            Quit();
        }   
    }
    
}

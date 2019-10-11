using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    // rewired
    private Rewired.Player rwplayer;

    public void Start(){
        rwplayer = Rewired.ReInput.players.GetPlayer(0);
    }

    public void LoadScene(int level) {
        SceneManager.LoadScene(level);
    }

    void Update()
    {
        if(rwplayer.GetButtonDown("Pause")){
            LoadScene(1);
        }   
    }
}

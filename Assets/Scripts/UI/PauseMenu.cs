using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject MinimapUI;
    public GameObject[] HealthandManaUI;
    // rewired
    private Rewired.Player rwplayer;

    void Start()
    {
        rwplayer = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(rwplayer.GetButtonDown("Pause"))
        {
            ////Debug.Log("hi");
            if (GameIsPaused)
                {
                    Resume();
                }
                else 
                {
                    Paused();
        }       }
    }

   public void Resume() 
    {
        MinimapUI.SetActive(true);
        foreach(GameObject ui in HealthandManaUI)
        {
            ui.SetActive(true);
        }
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Paused()
    {
        MinimapUI.SetActive(false);
        foreach (GameObject ui in HealthandManaUI)
        {
            ui.SetActive(false);
        }
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void QuitGame() {
       
        Application.Quit();
    }
}

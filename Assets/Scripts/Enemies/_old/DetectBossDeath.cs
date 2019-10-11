using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DetectBossDeath : Singleton<DetectBossDeath>
{
    public Text winScreen;
    public GameObject teleporter;
    public GameObject escapeJawn;

    void Update() {
        if (escapeJawn.activeSelf == false) {
            SpawnTeleporter();
        }
    }

    public void SpawnTeleporter() {
        teleporter.SetActive(true);
    }




    // Start is called before the first frame update
    /* void Start()
     {
         winScreen = GameObject.FindGameObjectWithTag("Winner").GetComponent<Text>();
     }

     public void ShowEndScreen()
     {
         winScreen.enabled = true;
         StartCoroutine(waiting(5));
     }

     IEnumerator waiting(int seconds)
     {
         yield return new WaitForSeconds(seconds);
         SceneManager.LoadScene(0);
     }
     */
}

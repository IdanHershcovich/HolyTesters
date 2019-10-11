using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_How_To_Play : MonoBehaviour
{
    public GameObject playscreen;
    

    public void openScreen()
    {
        playscreen.SetActive(true);
    }

    public void closeScreen()
    {
        playscreen.SetActive(false);
    }
}

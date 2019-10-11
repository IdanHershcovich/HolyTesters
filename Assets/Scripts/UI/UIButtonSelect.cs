using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSelect : MonoBehaviour
{
    public RawImage grailSelect;
   
    public void showGrail()
    {
        grailSelect.enabled = true;
    }

    public void hideGrail()
    {
        grailSelect.enabled = false;
    }
}

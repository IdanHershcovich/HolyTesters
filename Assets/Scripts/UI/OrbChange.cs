using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbChange : MonoBehaviour
{
    public Image FillImage;
    public Sprite WhiteOrb;
    public Sprite ColorOrb;
    public float waitTime = .5f;
    //public float flickerTime = .1f;
    public bool colorOn = false;

    public void changeColor(float n)
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        int i = 0;
        while (i < 5)
        {
            if (colorOn == false)
            {
                FillImage.sprite = WhiteOrb;
            }
            else
            {
                FillImage.sprite = ColorOrb;
            }
            colorOn = !colorOn;
            yield return new WaitForSeconds(waitTime);
            i++;
        }
        FillImage.sprite = ColorOrb;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Floating Text Controller courtesy of https://www.youtube.com/watch?v=fbUOG7f3jq8
/// </summary>
public class FloatingTextController : MonoBehaviour
{
    public static GameObject canvas;

    public void Awake()
    {
        canvas = gameObject;
    }

    /// <summary>
    /// Create a new Floating Text object at object location
    /// </summary>
    /// <param name="text">the text in the Floating text object</param>
    /// <param name="location">the location where to spawn the text</param>
    public static void CreateFloatingText(string text, Transform location)
    {
        GameObject floatText = ObjectPooler.SharedInstance.GetPooledObject("DamageText");
        if(floatText != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x, location.position.y));
            //floatText.transform.SetParent(canvas.transform, true);
            floatText.transform.position = new Vector3(location.position.x, location.position.y + 1.0f, location.position.z);//screenPosition;
            
            floatText.GetComponentInChildren<FloatingText>().SetText(text);
            floatText.SetActive(true);
            floatText.GetComponentInChildren<FloatingText>().StartAnim();
            
        }
    }

    /// <summary>
    /// Turn off the Floating Text and return it to the objectPool
    /// </summary>
    /// <param name="text">Floating text to turn off</param>
    public static void TurnOffFloatingText(GameObject text)
    {
        if(text.tag == "DamageText")
        {
            text.SetActive(false);
        }
    }
}

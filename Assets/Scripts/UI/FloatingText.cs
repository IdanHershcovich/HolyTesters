using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Animator animator;
    public GameObject parent;

    public void SetText(string text)
    {
        try{
            damageText.text = text.Split('.')[0];
            return;
        }
        finally{

        }
        damageText.text = text;
    }

    public void StartAnim()
    {
        animator.SetTrigger("Play");
    }
    public void EndText()
    {
        FloatingTextController.TurnOffFloatingText(parent);
    }
}

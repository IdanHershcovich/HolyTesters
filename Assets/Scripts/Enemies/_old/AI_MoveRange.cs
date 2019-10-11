using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
    Created by Nick Tang 
    4/30/19
*/

public class AI_MoveRange : MonoBehaviour
{
    EnemyNavAgent parentAgent;
    [SerializeField] private Slider hpBar; //Hp bar of enemy
    [SerializeField] private Image background; //UI element of hp bar
    [SerializeField] private Image fill; //UI element of hp bar
    // Start is called before the first frame update
    void Start()
    {
        background = background.GetComponent<Image>();
        fill = fill.GetComponent<Image>();
        hpBar.enabled = false;
        background.enabled = false;
        fill.enabled = false;
        parentAgent = this.gameObject.GetComponentInParent<EnemyNavAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When player is in range of enemy, turn on HP bar and set AI state to target the player
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            hpBar.enabled = true;
            background.enabled = true;
            fill.enabled = true;
            if(parentAgent != null) {
                parentAgent.setState("target");
            }
            
        }
    }
    // When player exits the range of the enemy, turn off HP bar and set AI state to wander
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            hpBar.enabled = false;
            background.enabled = false;
            fill.enabled = false;
            parentAgent.setState("wander");
        }
    }
}

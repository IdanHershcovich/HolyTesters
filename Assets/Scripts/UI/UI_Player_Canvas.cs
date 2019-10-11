using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using TMPro;


public class UI_Player_Canvas : MonoBehaviour
{
    [SerializeField] private Slider superBar;
    [SerializeField] private GameObject inventoryObject;
    private Inventory_Reworked inventory;
    private float counter = 0;
    private float superBarcounterPeriod = .01f;
    public static UI_Player_Canvas Instance;

    /// <summary> ReWired Player object </summary>
    private Rewired.Player rwplayer;

    public TextMeshProUGUI grailUIDisplay;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
        rwplayer = ReInput.players.GetPlayer(0);
        //Serialize this when startroom.prefab is available to edit
        inventory = Player.Instance.inventory;
    }

    // Update is called once per frame
    void Update()
    {
        grailUIDisplay.text = inventory.getEquippedGrail().getName();

        counter += Time.deltaTime;
        if (counter >= superBarcounterPeriod)
        {
            tickEquippedSuperBar();
            counter = 0;
        }
        /*        //Hardcoded keys to test and debug
        if (rwplayer.GetButtonDown("Slam"))
        {
            inventory.useSuper();
        } */

       

        if (rwplayer.GetButtonDown("Switch Backward"))
        {
            inventory.swapForward();
            updateSuperBarUI();
      
        }
        if (rwplayer.GetButtonDown("Switch Forward"))
        {
            inventory.swapBackward();
            updateSuperBarUI();
        }
    }


    void tickEquippedSuperBar()
    {
        if (inventory.getEquippedGrail().getName() != inventory.nullGrail.getName())
        {
            inventory.inventory[0].addSuperBarPercent();
            updateSuperBarUI();
        }
    }

    void updateSuperBarUI() {
        superBar.value = inventory.inventory[0].getSuperBarPercent();
    }
}

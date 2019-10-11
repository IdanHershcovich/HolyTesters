using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_Inventory : MonoBehaviour
{
    //rotating object source
    //https://answers.unity.com/questions/1441319/rotate-a-game-object-exactly-90-degrees-with-anima.html

    public GameObject objectToRotate;
    private bool rotating;
    public float rotationAmount;
    public float rotationSpeed;
    public static UI_Inventory Instance;
    public GameObject slot0;
    public GameObject slot1;
    public GameObject slot2;
    [SerializeField] Inventory_Reworked inventory;

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

        inventory = Inventory_Reworked.Instance;
        rotationSpeed = inventory.switchCooldown;
    }
    void Update() {
        slot0.GetComponent<UI_Grail_Slot>().setName(inventory.inventory[0].getName());
        slot1.GetComponent<UI_Grail_Slot>().setName(inventory.inventory[1].getName());
        slot2.GetComponent<UI_Grail_Slot>().setName(inventory.inventory[2].getName());
    }
    private void Rotate(float rotation)
    {
        rotating = true;
        iTween.RotateBy(objectToRotate, iTween.Hash("rotation", new Vector3(0, rotation, 0), "time", rotationSpeed, "easetype", iTween.EaseType.easeInOutSine));
        rotating = false;
        
    }

    public void rotateRight()
    {
        if (!rotating)
            Rotate(rotationAmount);
    }

    public void rotateLeft()
    {
        if (!rotating)
            Rotate(-rotationAmount);
    }

    public float getSwitchSpeed() {
        return rotationSpeed;
    }

    public void updateAllSlots() {
        slot0.GetComponent<UI_Grail_Slot>().updateModels(inventory.inventory[0].getName());
        slot1.GetComponent<UI_Grail_Slot>().updateModels(inventory.inventory[1].getName());
        slot2.GetComponent<UI_Grail_Slot>().updateModels(inventory.inventory[2].getName());
    }
}

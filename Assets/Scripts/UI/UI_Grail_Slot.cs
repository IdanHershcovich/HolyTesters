using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Grail_Slot : MonoBehaviour
{
    public string grailName;
    public List<GameObject> grailModelsSlot;
    // Start is called before the first frame update


    void Start()
    {
        grailName = "";
    }

    

    public string getName()
    {
        return grailName;
    }
    public void setName(string target) {
        grailName = target;
    }

    public void clearName() {

        grailName = "";
    }
    public void updateModels(string name)
    {

        for (int j = 0; j < this.transform.childCount - 1; j++)
        {
            this.transform.GetChild(j).gameObject.SetActive(false);
        }

        switch (name) {
            case "Vampire_Grail":
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Mech_Grail":
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "Light_Grail":
                this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case "Ice_Grail":
                this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case "Electric_Grail":
                this.gameObject.transform.GetChild(4).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}

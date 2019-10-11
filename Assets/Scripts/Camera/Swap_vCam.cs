using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap_vCam : MonoBehaviour
{
    public GameObject overHeadVCAM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //if (collision.gameObject.CompareTag("Player")){
        //    overHeadVCAM.SetActive(true);
        //}
    }
    void OnCollisionStay(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    overHeadVCAM.SetActive(true);
        //}
    }
    void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Player")){
        //    overHeadVCAM.SetActive(false);
        //}
    }
}

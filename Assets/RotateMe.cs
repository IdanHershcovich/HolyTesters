using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{

    public int direction = 1;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0,direction * 10,0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    public GameObject minimapDoor;
      void OnDrawGizmos()
    {
     Ray ray = new Ray (transform.position, transform.rotation * Vector3.forward);
     Gizmos.color = Color.red;
     Gizmos.DrawRay(ray);
    }

    public void turnOnDoor()
    {
        minimapDoor.SetActive(true);
    }
}
    
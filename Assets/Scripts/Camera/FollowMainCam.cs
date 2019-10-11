using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCam : MonoBehaviour
{
    Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
         this.transform.SetPositionAndRotation(m_Camera.GetComponent<Transform>().transform.position, m_Camera.GetComponent<Transform>().transform.rotation);
    }
}

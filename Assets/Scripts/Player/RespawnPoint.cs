using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    void Awake() {
        Player.Instance.transform.position = this.transform.position;
            
    }
}

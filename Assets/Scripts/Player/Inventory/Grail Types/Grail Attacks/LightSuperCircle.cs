using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSuperCircle : MonoBehaviour
{
    private float timer=0f;
    private float duration = 10f;
    private float radiusCoeff = 1f;
    private float drainRate = .1f;
    private bool playerIsIn = true;
    // for checking if player has light buff
    private LightBuff lbref;
    private Rewired.Player rwplayer;
    private Vector3 originalScale;

    void Start(){
        originalScale = gameObject.transform.localScale;
        lbref = new LightBuff(200f);
        rwplayer = Rewired.ReInput.players.GetPlayer(0);

    }

    void FixedUpdate(){
        timer +=  Time.fixedDeltaTime;
        if(timer > duration){
            DestroySelf();
        }
        // if the player has a light buff and they are attacking, they are probably in the circle: Shrink the circle
        if(playerIsIn && rwplayer.GetButton("Fire1")){
            radiusCoeff -= drainRate * Time.fixedDeltaTime;
            SetScale();
        }
    }

    private void SetScale(){
        gameObject.transform.localScale = originalScale * radiusCoeff;
        if(radiusCoeff < .2f){
            DestroySelf();
        }
    }

    private void DestroySelf(){
        Player.Instance.RemoveDebuff(lbref);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        
        //if player is hit, damage player
        if (col.gameObject.tag == "Player" )
        {
            playerIsIn = true;
            Player.Instance.RegisterDebuff(new LightBuff(duration - timer));
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" )
        {
            playerIsIn = false;
            Player.Instance.RemoveDebuff(new LightBuff(duration - timer));
        }
    }
}

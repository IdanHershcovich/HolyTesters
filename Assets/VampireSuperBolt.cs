using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireSuperBolt : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 target;
    private float timer = 0f;
    private float maxTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = Player.Instance.transform.position;   
        gameObject.transform.position += (target - gameObject.transform.position) * speed * Time.fixedDeltaTime;
        timer += Time.fixedDeltaTime;

                //if timer exceeds max time, destroy object
        if (timer >= maxTime || (Player.Instance.transform.position - gameObject.transform.position).magnitude < 1f)
        {
            Player.Instance.Heal(GetHealModifier() * 1f);
            Destroy(gameObject);
        }
            
    }

    private float GetHealModifier(){
        float percentHealth = Player.Instance.GetPercentHealth();
        float percentEmpty = 1f-percentHealth; // ranges from 0 at full health to 1 at 0 health
        return( 1f + (3 * percentEmpty) * (3 * percentEmpty) );
    }
}

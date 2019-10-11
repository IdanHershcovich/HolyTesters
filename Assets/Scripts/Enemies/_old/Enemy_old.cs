using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
    Created by Nick Tang 
    4/23/19
*/

public class Enemy_old : MonoBehaviour
{
    public int health;
    public int maxHealth = 20;
    public Slider healthBar;

    private bool _isStunned;
    /// <summary> Determines if the enemy is able to move and make an action </summary>
    public bool isStunned { get { return _isStunned; } private set { _isStunned = value; } }
    /// <summary> The reference to the escape prevention object in the room </summary>
    [SerializeField] private EscapePrevention escapePrevent;
    private Renderer model;



    [SerializeField]
    private float
        kbDecel,
        kbStunTime,
        attackCoolDownTime;

    private ObjectMover mover;
    private EnemyNavAgent enemyNavAgent;
    AudioSource audioSource;

    [SerializeField]
    AudioClip
        deathSFX;

    void Start()
    {
        model = this.GetComponentInChildren<SkinnedMeshRenderer>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        mover = GetComponent<ObjectMover>();
        enemyNavAgent = GetComponent<EnemyNavAgent>();
        escapePrevent = transform.parent.GetComponentInChildren<EscapePrevention>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health <= 0) {
            die();
        }
    }
    

    void FixedUpdate() {
        healthBar.value = health;
    }

    public void takeDamage(int attackPower) {
        health -= attackPower;
    }

    public void TakeKnockBack(Vector2 direction, float speed)
    {
        StartCoroutine("KnockBack", direction * speed);
    }

    private IEnumerator KnockBack(Vector2 velocity)
    {
        Stun();

        for (float currSpeed = velocity.magnitude ; currSpeed > 0; currSpeed -= kbDecel * Time.fixedDeltaTime)
        {
            mover.Move(velocity.normalized * currSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }


        for (float timer = 0; timer < kbStunTime; timer += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        Unstun();

    }

    public void InitiateAttackCoolDown()
    {
        StartCoroutine("AttackCoolDown");
    }

    private IEnumerator AttackCoolDown()
    {
        Stun();

        for (float timer = 0; timer < attackCoolDownTime; timer += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        Unstun();

    }

    /*
    private void OnTriggerEnter(Collider other)
    {

        Color NormalColor = Color.red;
        Color FlashColor = Color.black;
        
        if (other.gameObject.tag == "PlayerAttack")
        {
            //Debug.Break();
            ////Debug.Log("Hit Enemy");

            //Camera shake
            Camera.main.gameObject.GetComponent<CameraScript>().StartCameraShake(0.5f, 0.75f, 2f);

            //Hit stop call for combat.
            FindObjectOfType<HitStop>().Stop(0.09f);
            float duration = 0.5f;
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            model.material.color = Color.Lerp(NormalColor, FlashColor, lerp);

            PlayerAttack player = other.GetComponentInParent<PlayerAttack>();
            //Enemy take damage
            health -= player.getPlayerPower();
            Vector2 direction;
            if (other.name == "Slam")
            {
                direction = Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up).normalized;
                health -= player.getPlayerPower();
                health -= player.getPlayerPower();
                //TakeKnockBack(direction, player.pushSpeed * 2);
            }
            else
            {
                direction = new Vector2(player.transform.forward.x, player.transform.forward.z);
                //Enemy gets pushed away from player
                TakeKnockBack(direction, player.pushSpeed);
            }
        }
    }
    */

    void OnParticleCollision(GameObject other) {
        if (other.name == "Particles_grailLightning") {
            health -= maxHealth / 2;
        }
    }

    /// <summary>
    /// stuns the enemy and informs the navmesh agent
    /// </summary>
    public void Stun()
    {
        isStunned = true;
        enemyNavAgent.Stun();
    }

    /// <summary>
    /// unstuns the enemy and informs the navmesh agent
    /// </summary>
    public void Unstun()
    {
        isStunned = false;
        enemyNavAgent.Unstun();
    }

    void die() {
        // Play sound effect
        SoundController.Instance.PlaySoundEffect(deathSFX);

        // informs the escape prevention object that the enemy has died
        // removes it from the list of enemies attached to that object
        //escapePrevent.GetComponent<EscapePrevention>().RemoveEnemy(this);

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, Buffable
{
    private DebuffList debuffs = new DebuffList();

    //Added by Rush on 7/28 for health orbs
    public UnityEvent enemyDeath;

    //Get transform of associated enemy for spawn location purposes.
    public GameObject healOrb;
    //Percent chance an orb spawns on an enemy death
    [Range(0, 100)]
    public float percentChanceHealOrbSpawns;
    [Range(0, 3)]
    //Trying to get it to spawn at the right height every time
    public float healOrbspawnHeight;

    [SerializeField]
    private bool cancelStun;

    /// <summary>
    /// Starting health
    /// </summary>
    [SerializeField]
    public float maxHealth;

    void Update(){
        debuffs.ApplyDebuffEffects();
    }

    /// <summary>
    /// Current health
    /// </summary>

    public float health;

    // TODO: make enemy execute different tactic in different situations,
    // rather than just setting this one active/inative
    /// <summary>
    /// Enemy only executes tactic if active
    /// </summary>
    public bool active;
    
    /// <summary>
    /// Currently has iFrames
    /// </summary>
    private bool hasIFrames;

    /// <summary>
    /// Default iFrame time
    /// </summary>
    [SerializeField]
    private float defaultIFrameTime;

    [SerializeField]
    private Tactic tactic;

    public List<Tactic> possibleTatics;

    #region components
    /// <summary> MeshRenderer that holds the material for this enemy. Necessary for some effects. </summary>
    [SerializeField]
    SkinnedMeshRenderer meshRenderer;
    /// <summary>
    /// Reference to player
    /// </summary>
    Player player;

    /// <summary>
    /// Reference to this enemy's animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Used for managing attacks
    /// </summary>
    private EnemyAttackComponent attackComponent;

    /// <summary>
    /// Used to update the enemy HealthBar
    /// </summary>
    public HealthBar healthbar;

    /// <summary>
    /// Used for directing movement
    /// </summary>
    private EnemyMoveComponent moveComponent;

    /// <summary>
    /// Used for producing visual effects (particle systems, popups, etc)
    /// </summary>
    private EnemyVFXComponent vfxComponent;

    /// <summary>
    /// Arbitrary int for comparing enemies
    /// </summary>
    public int serialNo;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        serialNo = (int)Random.Range(0f,10000f);
        // Initialize health
        health = maxHealth;

        // Get reference to components
        animator = GetComponentInChildren<Animator>();
        moveComponent = GetComponent<EnemyMoveComponent>();
        attackComponent = GetComponent<EnemyAttackComponent>();
        vfxComponent = GetComponent<EnemyVFXComponent>();

        // Select tactic at random if no default
        if(tactic == null) {
            tactic = GetRandomTactic();
        }

        // Instantiate and initialize tactic as child object
        tactic = Instantiate(tactic, transform);
        tactic.Initialize(this);
    }

    void OnEnable()
    {
        // Play spawn VFX
        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Plays spawn VFX (makes enemy stay in place until effect has finished playing)
    /// </summary>
    private IEnumerator Spawn()
    {
        vfxComponent.PlaySpawnVFX();

        // Make enemy wait a bit before targeting the player
        // TODO: play some sort of gooey sound effect here, like a screech or something (should vary per enemy)
        active = false;
        float idleTime = 1f;
        yield return new WaitForSeconds(idleTime);
        active = true;
    }

    /// <summary>
    /// Switches to new tactic
    /// </summary>
    /// <param name="newTactic">Tactic to switch to</param>
    /// <param name="interrupt">If true, tactic in progress is interrupted immediately. If false, waits until end of tactic loop to switch tactics</param>
    public void SetTactic(Tactic newTactic, bool interrupt = false)
    {
        // Interrupt tactic in progress
        if(interrupt) {
            SwitchTactic(newTactic);
        }
        // Or wait until end of tactic loop
        else {
            tactic.onTacticLoopEnd += () => SwitchTactic(newTactic);
        }
    }

    /// <summary>
    /// Removes current tactic and initializes new one
    /// </summary>
    /// <param name="newTactic"></param>
    private void SwitchTactic(Tactic newTactic)
    {
        // Remove current tactic
        Destroy(tactic);

        // Instantiate and initialize new tactic as child object
        tactic = Instantiate(newTactic, transform);
        tactic.Initialize(this);
    }

    /// <summary>
    /// Returns a random tactic that can be used by this enemy
    /// </summary>
    public Tactic GetRandomTactic()
    {
        return possibleTatics[Random.Range(0, possibleTatics.Count)];
    }

    // apply a debuff
    public void RegisterDebuff(Debuff db){
        db.SetTarget(this);
        debuffs.RegisterDebuff(db);
    }
    // remove a debuff
    public void RemoveDebuff(Debuff db){
        debuffs.RemoveDebuff(db);
    }

    /// <summary>
    /// Subtracts health from Enemy
    /// </summary>
    /// <param name="damage">Health to be subtracted</param>
    /// <param name="iFrameTimeMod">Modifier for iFrame time, -1 for no modification</param>
    public void TakeDamage(float damage, float iFrameTimeMod = -1)
    {

        //Debug.LogFormat("{0} took {1} damage (health: {2})", name, damage, health);
        health -= damage;
        healthbar.setHealth((float)health / maxHealth);
        if (health < 0)
        {
            //enemyDeath.Invoke();
            health = 0;
            Die();
        }


        StartCoroutine(StartIFrames(iFrameTimeMod));
    }

    private IEnumerator StartIFrames(float iFrameTimeMod = -1)
    {
        if (hasIFrames)
            yield break;

        //SkinnedMeshRenderer meshRend = GetComponentInChildren<SkinnedMeshRenderer>();

        hasIFrames = true;

        float j = 0; ;
        for (float i = 0; i < ((iFrameTimeMod == -1) ? defaultIFrameTime : iFrameTimeMod); i += Time.deltaTime)
        {
            //if (j >= .2)
                //meshRend.enabled = !meshRend.enabled;

            yield return null;
            j += Time.deltaTime;
        }

        //meshRend.enabled = true;

        hasIFrames = false;
    }

    public void SpawnOrb()
    {
        //randomize a number, if it is smaller than our threshold, spawn an orb. 
        float random = Random.Range(0, 100);
        if (random <= percentChanceHealOrbSpawns)
        {
            Debug.Log("Orb Spawned");
            /*
            Transform spawnSpot = t;
            Debug.Log("Spawn spot: " + spawnSpot);
            Vector3 temp = new Vector3(.5f, spawnHeight, .5f);
            spawnSpot.position += temp;
            */
            //HealingOrb orb = Instantiate(healOrb, t).GetComponent<HealingOrb>();
            GameObject orb = Instantiate(healOrb, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Triggers enemy death
    /// Called when health == 0
    /// </summary>
    public void Die()
    {
        enemyDeath.Invoke();

        // Play death VFX/sound effect
        vfxComponent.PlayDeathVFX();
        SoundController.Instance.PlaySoundEffect(SoundType.STEAMON_DEATH);

        // Update EscapePrevention (removes this enemy from list)
        transform.parent.GetComponentInChildren<EscapePrevention>().RemoveEnemy(this);

        SpawnOrb();
        Player.Instance.FillSuper(15);

        // Destory self
        Destroy(gameObject);
    }

    /// <summary>
    /// Stuns the enemy for the given time
    /// </summary>
    /// <param name="time">Stun time length</param>
    /// <returns></returns>
    public IEnumerator Stun(float time)
    {
        animator.SetTrigger("HitStun");
        if (time <= 0)
            yield break;
        
        active = false;
        yield return new WaitForSeconds(time);
        active = true;
    }

    /// <summary>
    /// Stuns and applies a knockback impulse on Enemy with an optional extra stun time
    /// </summary>
    /// <param name="knockback">The impulse (initial velocity) to be applied to the enemy</param>
    /// <param name="extraStunTime">Extra time for Enemy to remain stunned after knockback ends</param>
    public void TakeKnockback(Vector3 knockback, float extraStunTime = 0)
    {
        if (!cancelStun)
            StartCoroutine(Stun(extraStunTime));

        moveComponent.TakeKnockback(knockback);
    }

    /// <summary>
    /// Deals an attack on Enemy
    /// </summary>
    /// <param name="hitAttr">Struct containing attack info</param>
    public void ReceiveAttack(HitAttr hitAttr)
    {
        
        if (hasIFrames)
            return;

        if (hitAttr.damage != 0)
        {
            FloatingTextController.CreateFloatingText(hitAttr.damage.ToString(), this.transform);
            TakeDamage(hitAttr.damage, hitAttr.iFrameTimeMod);
        }

        if (hitAttr.knockback != Vector3.zero)
        {
            active = false;
            TakeKnockback(hitAttr.knockback, hitAttr.stunTime);
        }
        else if (hitAttr.stunTime != 0 && !cancelStun)
            StartCoroutine(Stun(hitAttr.stunTime));

    }

    public void Tint(Color color, float amount)
    {
        if (meshRenderer != null)
        {
            try
            {
                meshRenderer.material.SetColor(Shader.PropertyToID("_TintColor"), color);
                meshRenderer.material.SetFloat(Shader.PropertyToID("_Tint"), amount);
                meshRenderer.material.SetFloat(Shader.PropertyToID("_TintAmount"), 1); //Shader booleans must be floats, 1 just means true
            }
            catch
            {
                Debug.LogWarning("Tried to tint a monster and failed.");
            }
        }
    }

    public void UnTint()
    {
        if (meshRenderer != null)
        {
            try
            {
                meshRenderer.material.SetFloat(Shader.PropertyToID("_TintAmount"), 0); //Shader booleans must be floats, 0 just means false
            }
            catch
            {
                Debug.LogWarning("Tried to untint a monster and failed.");
            }
        }
    }

    public bool HasDebuff(Debuff db){
        return debuffs.HasDebuff(db);
    }
    

    // used for debuffs
    public void SetMoveSpeedMultiplier(float f){
        moveComponent.SetMoveSpeedMultiplier(f);
    }

    public void TakeDebuffDamage(float f){
        TakeDamage(f,0);
    }


    #region getters

    public Animator GetAnimator() {
        return animator;
    }

    public EnemyMoveComponent GetMoveComponent() {
        return moveComponent;
    }

    public EnemyAttackComponent GetAttackComponent() {
        return attackComponent;
    }

    public EnemyVFXComponent GetVFXComponent() {
        return vfxComponent;
    }

    #endregion
}

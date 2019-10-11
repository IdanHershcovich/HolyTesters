
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_BasicAttack : MonoBehaviour
{

    /// <summary> The damage to the player </summary>
    [SerializeField] private float attackPower;
    /// <summary> The initial knockback speed to player </summary>
    [SerializeField] private float pushSpeed;

    /// <summary> The end lag time for the enemy attack, starts after animation ends </summary>
    [SerializeField] private float attackEndLagTime;
    /// <summary> The timer for the attack end lag </summary>
    private float attackEndLagTimer;

    /// <summary> Bool for if the player is still in the attack animation</summary>
    private bool isAttacking;

    /// <summary> 
    /// Temporary bool used to determine when player gets atttacked when in range
    /// Will be obsolete when implementation of separate attack hitbox is completed
    /// </summary>
    private bool attackActive;

    Enemy_old status;
    Animator animator;
    EnemyNavAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        status = GetComponentInParent<Enemy_old>();
        nav = GetComponentInParent<EnemyNavAgent>();
    }

    /*
    /// <summary>
    /// Applies damage and knockback to the player
    /// </summary>
    /// <param name="player"> Reference to the player's status script</param>
    public void AttackPlayer(PlayerStatus player)
    {
        // Apply damage to player
        player.TakeDamage(attackPower);

        // Knock back player
        player.TakeKnockBack(new Vector2(transform.forward.x, transform.forward.z), pushSpeed);

        // Attack cooldown
        status.InitiateAttackCoolDown();
    }
    */

    /// <summary>
    /// Activates the hitbox of the attack to possibly collide with the player
    /// Intended to be called via animation event 
    /// Will be changed to activate the hitbox, for now uses soon to be
    /// deprecated system that has the attack range count as the hitbox
    /// </summary>
    public void ActivateAttackHitbox()
    {
        attackActive = true;
    }

    /// <summary>
    /// Deactivates the hitbox of the attack to no longer collide with the player
    /// Intended to be called via animation event 
    /// Will be changed to activate the hitbox, for now uses soon to be
    /// deprecated system that has the attack range count as the hitbox
    /// </summary>
    public void DeactivateAttackHitbox()
    {
        attackActive = false;
    }

    /// <summary>
    /// Sets the attacking bool to false and starts end lag coroutine
    /// Intended to be called via animation event when the attack animation ends
    /// </summary>
    public void OnAnimationEnd()
    {
        isAttacking = false;
        StartCoroutine(AttackEndLag());
    }

    private IEnumerator AttackEndLag()
    {
        while (attackEndLagTimer < attackEndLagTime)
        {
            attackEndLagTimer += Time.deltaTime;
            yield return null; 
        }

        attackEndLagTimer = 0;
    }

    void OnTriggerStay(Collider other) {

        if (other.CompareTag("Player"))
        {
            if (!isAttacking && attackEndLagTimer == 0)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }

            if (attackActive)
            {
                //AttackPlayer(other.gameObject.GetComponent<PlayerStatus>());
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeAttack : Attack
{
    #region fields

    /// <summary>
    /// Trigger collider
    /// </summary>
    private Collider hitbox;

    private bool touchingPlayer;

    /// <summary>
    /// Particle system vfx to play on successful hit.
    /// </summary>
    [SerializeField]
    private ParticleSystem hitVFX;

    #endregion

    Coroutine attackCoroutine;

    private void Start()
    {
        hitbox = GetComponent<Collider>();
    }

    public override void Execute(Enemy enemy, Transform target)
    {
        //Debug.Log("MeleeAttack.Execute()");
        if(attackCoroutine == null) {
            attackCoroutine = StartCoroutine(Attack(enemy, target));
        }
    }

    IEnumerator Attack(Enemy enemy, Transform target)
    {
        // Get animator and set attack trigger
        Animator animator = enemy.GetAnimator();
        animator.SetTrigger("Attack");
        SoundController.Instance.PlaySoundEffect(SoundType.STEAMON_ATTACK);

        // Loop during attack duration
        while (true) {
            // If attack is set active by anim event, check if player is in range
            if(active && InRangeOf(target)) {
                // Apply attack effects to player
                PlayHitVFX();
                ApplyEffectsToPlayer(enemy, Player.Instance);

                // Stop checking for player contact
                break;
            }
            yield return null;
        }
    }

    public override void End()
    {
        //Debug.Log("EndAttack()");
        // Stop current attack coroutine
        StopCoroutine(attackCoroutine);

        // Disable effects of attack
        active = false;
        attackCoroutine = null;
    }

    public override bool InRangeOf(Transform target)
    {
        // Check for any collisions with hitbox from Player layer
        int layermask = 1 << LayerMask.NameToLayer("Player");
        return Physics.CheckBox(transform.position, hitbox.bounds.extents, transform.rotation, layermask, QueryTriggerInteraction.Collide);
    }

    private void PlayHitVFX()
    {
        if (hitVFX != null)
            hitVFX.Play();
    }
}
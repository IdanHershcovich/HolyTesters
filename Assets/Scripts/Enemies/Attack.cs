using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    #region fields

    /// <summary>
    /// Damage done to player
    /// </summary>
    public float damage;

    /// <summary>
    /// Amount of force to apply to the player
    /// </summary>
    public float knockBackSpeed;

    /// <summary>
    /// The time the player remains stunned after knockback
    /// </summary>
    public float stunTime;
    
    /// <summary>
    /// When true, attack will deal damage/effects on contact with player (via trigger contact, raycast, etc)
    /// </summary>
    protected bool active = false;

    /// <summary>
    /// Speed at which attack occurs (affects both animation and functionality)
    /// </summary>
    [Range(0, 5)]
    public float speed = 1;

    #endregion
    /// <summary>
    /// Does the attack
    /// </summary>
    /// <param name="enemy">Enemy using the attack</param>
    /// <param name="target">Target of the attack</param>
    public abstract void Execute(Enemy enemy, Transform target);

    /// <summary>
    /// Returns true if attack can connect with target, false otherwise
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public abstract bool InRangeOf(Transform target);

    /// <summary>
    /// Event called via Animator Events when attack becomes "lethal" (probably shouldn't be called in other places)
    /// </summary>
    public void SetHitboxActive(bool active)
    {
        this.active = active;
    }

    /// <summary>
    /// Event called via Animator Events at end of attack (probably shouldn't be called in other places)
    /// </summary>
    public virtual void End()
    {
        SetHitboxActive(false);
    }

    /// <summary>
    /// When called, the attack effects (damage, knockback, etc) are applied to the player
    /// </summary>
    /// <param name="enemy">Enemy using the attack</param>
    /// <param name="player">Player hit by the attack</param>
    protected void ApplyEffectsToPlayer(Enemy enemy, Player player)
    {
        //Debug.LogWarningFormat("{0}'s attack hit Player", enemy.name);

        HitAttr hitAttr = new HitAttr(damage,knockBackSpeed * enemy.transform.forward, stunTime);

        // TODO: Apply to player

        player.ReceiveAttack(hitAttr);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// More Modular version of Animation Events for Enemies -Michelle Zhong
/// </summary>
public class EnemyAnimEvents_old : MonoBehaviour
{
    /// <summary> The script holding the data and functions to work with the Enemy's attacks 
    /// Note: change this to whatever Drew puts up for attacks later...
    /// </summary>
    public AI_BasicAttack attackScript;

    /// <summary>
    /// Turns on the hitbox collider for enemy attacks during the attack animation
    /// </summary>
    public void TurnAttackCollisionOn()
    {
        attackScript.ActivateAttackHitbox();
    }

    /// <summary>
    /// Turns off the hitbox collider for Enemy during the attack animation
    /// </summary>
    public void TurnAttackCollisionOff()
    {
        attackScript.DeactivateAttackHitbox();
    }

    /// <summary>
    /// Notifies the Enemy AI script when the attack animation finishes
    /// </summary>
    public void EndAttack()
    {
        attackScript.OnAnimationEnd();
    }
}

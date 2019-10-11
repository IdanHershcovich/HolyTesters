using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing animation events for Lesser Demon
/// author: Michelle Zhong
/// </summary>
public class LesserDemonAnimEvents : MonoBehaviour
{

    /// <summary> The script holding the data and functions to work with the Lesser Demon's attacks </summary>
    AI_BasicAttack attackScript;

    private void Start()
    {
        attackScript = transform.parent.GetComponentInChildren<AI_BasicAttack>();
    }

    /// <summary>
    /// Turns on the hitbox collider for Lesser Demon attacks during the attack animation
    /// </summary>
    public void TurnAttackCollisionOn()
    {
        attackScript.ActivateAttackHitbox();
    }

    /// <summary>
    /// Turns off the hitbox collider for Lesser Demon during the attack animation
    /// </summary>
    public void TurnAttackCollisionOff()
    {
        attackScript.DeactivateAttackHitbox();

    }

    /// <summary>
    /// Notifies the Lesser Demon AI script when the attack animation finishes
    /// </summary>
    public void EndAttack()
    {
        attackScript.OnAnimationEnd();
    }
}

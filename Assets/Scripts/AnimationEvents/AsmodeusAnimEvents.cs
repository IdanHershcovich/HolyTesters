using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing animation events for Asmodeus
/// author: Michelle Zhong
/// </summary>
public class AsmodeusAnimEvents : MonoBehaviour
{
    /// <summary> The script holding the data and functions to work with Asmodeus' attacks </summary>
    AI_BasicAttack attackScript;

    private void Start()
    {
        attackScript = transform.parent.GetComponentInChildren<AI_BasicAttack>();
    }

    /// <summary>
    /// Turns on the hitbox collider for Asmodeus attacks during the attack animation
    /// </summary>
    public void TurnAttackCollisionOn()
    {
        attackScript.ActivateAttackHitbox();
    }

    /// <summary>
    /// Turns off the hitbox collider for Asmodeus during the attack animation
    /// </summary>
    public void TurnAttackCollisionOff()
    {
        attackScript.DeactivateAttackHitbox();
    }

    /// <summary>
    /// Notifies the Asmodeus AI script when the attack animation finishes
    /// </summary>
    public void EndAttack()
    {
        attackScript.OnAnimationEnd();
    }
}

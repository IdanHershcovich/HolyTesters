using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    #region fields

    /// <summary>
    /// Reference to attacks used at close range
    /// </summary>
    [SerializeField]
    protected List<Attack> shortRangeAttacks;


    /// <summary>
    /// Reference to attacks used at long range
    /// </summary>
    [SerializeField]
    protected List<Attack> longRangeAttacks;

    #endregion

    /// <summary>
    /// Reference to enemy this component is attacked to
    /// </summary>
    protected Enemy enemy;

    #region StartUpdate

    void Start()
    {
        // Get reference to enemy
        enemy = GetComponent<Enemy>();
    }

    #endregion

    /// <summary>
    /// Attack currently in progress
    /// </summary>
    private Attack currentAttack;

    /// <summary>
    /// Executes a short-range attack to be used on target
    /// </summary>
    /// <param name="target"></param>
    public void UseShortRangeAttack(Transform target)
    {
        // Use a random short range attack
        Attack attack = SelectRandomAttack(shortRangeAttacks);
        if(attack == null) {
            //Debug.LogWarning("Attempted to use short-range attack; none found for " + enemy.name);
        }
        else {
            UseAttack(attack, target);
        }
    }

    /// <summary>
    /// Executes a long-range attack to be used on target
    /// </summary>
    /// <param name="target"></param>
    public void UseLongRangeAttack(Transform target)
    {
        // Use a random long range attack
        Attack attack = SelectRandomAttack(longRangeAttacks);
        if(attack == null) {
            //Debug.LogWarning("Attempted to use long-range attack; none found for " + enemy.name);
        }
        else {
            UseAttack(attack, target);
        }
    }
    
    /// <summary>
    /// Executes attack on target
    /// </summary>
    private void UseAttack(Attack attack, Transform target)
    {
        // Use attack if target is in range
        if (attack.InRangeOf(target)) {
            currentAttack = attack;
            attack.Execute(enemy, target);
        }
    }

    /// <summary>
    /// Returns random attack from list (or null if list is empty)
    /// </summary>
    /// <param name="attacks">Attacks to select from</param>
    private Attack SelectRandomAttack(List<Attack> attacks)
    {
        if (attacks.Count > 0)
            return attacks[Random.Range(0, attacks.Count)];
        else return null;
    }

    #region getters_setters

    /// <summary>
    /// Returns attack currently being used, or null if enemy not attacking
    /// </summary>
    public Attack GetAttackInProgress()
    {
        return currentAttack;
    }

    /// <summary>
    /// Stops the attack behavior in progress (animation may still finish, but attack effects are cancelled)
    /// </summary>
    public void EndAttackInProgress()
    {
        // Defensive nullref catch
        if(currentAttack == null) {
            Debug.LogWarningFormat("{0}.EnemyAttackComponent.EndAttackInProgress(): no attack in progress", enemy.name);
        }
        else {
            currentAttack.End();
            currentAttack = null;
        }
    }

    #endregion
}

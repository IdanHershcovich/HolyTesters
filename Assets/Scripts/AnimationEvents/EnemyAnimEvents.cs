using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
    public Enemy enemy;

    [SerializeField]
    private GameObject Trails;
    
    public void SetAttackActive()
    {
        enemy.GetAttackComponent().GetAttackInProgress().SetHitboxActive(true);
    }

    public void SetAttackInactive()
    {
        //enemy.GetAttackComponent().GetAttackInProgress().SetHitboxActive(false);
        enemy.GetAttackComponent().EndAttackInProgress();
    }

    public void EndAttack()
    {
        enemy.GetAttackComponent().EndAttackInProgress();
    }

    public void TurnOnLineRenderers()
    {
        Trails.SetActive(true);
    }

    public void TurnOffLineRenderers()
    {
        Trails.SetActive(false);
    }
}

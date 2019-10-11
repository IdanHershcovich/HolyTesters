using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RetreatTactic : Tactic
{
    //Range used in our TacticManager to determine switching to Retreat. After retreat, we must determine
    //whether we should enter a HangBack or Surround Tactic.
    [Range(0, 10)]
    public float distanceAtWhichWeRetreat;

    public float waitTimeForNextMove;

    public float waitTimeForRangedAttack;

    public bool rangedEnemy;

    protected override IEnumerator TacticLoop()
    {
        // Find a position near target and move towards it
        destination = SamplePositionNearTarget(minDistanceFromTarget, minDistanceFromTarget);
        enemy.GetMoveComponent().MoveTowards(destination);

        while (!enemy.GetMoveComponent().HasReachedDestination())
            {
            // Rotate to face player
            moveComponent.RotateTowards(target);
            yield return null;
        }

        // Otherwise repeat after the wait-time is achieved
        yield return new WaitForSeconds(waitTimeForNextMove);
        tacticCoroutine = null;
        onTacticLoopEnd();
        yield return null;
    }
}

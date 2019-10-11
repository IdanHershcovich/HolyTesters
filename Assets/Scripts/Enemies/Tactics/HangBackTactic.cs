using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HangBackTactic : Tactic
{
    public float waitTimeForNextMove;

    public float waitTimeForRangedAttack;

    protected override IEnumerator TacticLoop()
    {
        // Find a position near target and move towards it
        destination = SamplePositionNearTarget(minDistanceFromTarget, minDistanceFromTarget);
        enemy.GetMoveComponent().MoveTowards(destination);

        while (!enemy.GetMoveComponent().HasReachedDestination()) {
            // Rotate to face player
            moveComponent.RotateTowards(target);
            yield return null;
        }

        // if a ranged enemy, attack. Otherwise, don't.
        yield return new WaitForSeconds(waitTimeForRangedAttack);
        attackComponent.UseLongRangeAttack(target.transform);

        // Otherwise repeat after the wait-time is achieved
        yield return new WaitForSeconds(waitTimeForNextMove);
        tacticCoroutine = null;
        onTacticLoopEnd();
        yield return null;
    }
}

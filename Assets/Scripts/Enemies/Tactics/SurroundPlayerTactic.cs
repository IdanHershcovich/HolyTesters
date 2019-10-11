using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurroundPlayerTactic : Tactic
{
    [SerializeField]
    private float WaitInPlace = .5f;

    

    protected override IEnumerator TacticLoop()
    {
        // Find a position near target and move towards it
        destination = SamplePositionNearTarget(minDistanceFromTarget, minDistanceFromTarget);
        enemy.GetMoveComponent().MoveTowards(destination);

        while (!enemy.GetMoveComponent().HasReachedDestination()) {
            // Rotate to face player
            moveComponent.RotateTowards(target);

            // Update path if target moves past beyond correction threshold
            if(pathCorrection > 0) {
                float correctionThreshold = pathCorrection + maxDistanceFromTarget;
                ApplyPathCorrection(correctionThreshold);
            }
            yield return null;
        }

        // If player is still there, attack after a few seconds
        attackComponent.UseShortRangeAttack(Player.Instance.transform);

        // Otherwise repeat after half a second
        yield return new WaitForSeconds(WaitInPlace);

        tacticCoroutine = null;
        onTacticLoopEnd();
        yield return null;
    }
}

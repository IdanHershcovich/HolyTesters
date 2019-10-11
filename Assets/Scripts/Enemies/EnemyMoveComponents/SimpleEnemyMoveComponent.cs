using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyMoveComponent : EnemyMoveComponent
{
    /// <summary>
    /// Current destination being moved towards
    /// </summary>
    protected Vector3 destination;

    /// <summary>
    /// Distance from the destination where enemy stops moving
    /// </summary>
    float stoppingDistance;

    public override bool HasReachedDestination()
    {
        // Check if enemy is within stopping distance
        Vector3 toDestination = (destination - transform.position);
        return toDestination.magnitude <= stoppingDistance;

        // Alternatively, we can just return movement == null
    }

    /// <summary>
    /// Initiates movement towards given target. 
    /// If enemy is already moving towards a target, sets path towards new target.
    /// </summary>
    /// <param name="target"></param>
    public override void MoveTowards(Vector3 dest)
    {
        // Save reference to destination
        destination = dest;
        // Just move directly towards destination until we reach it
        Vector3 move = (destination - transform.position).normalized;
        float speed = moveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime;

        objectMover.Move(move * speed);

    }
}

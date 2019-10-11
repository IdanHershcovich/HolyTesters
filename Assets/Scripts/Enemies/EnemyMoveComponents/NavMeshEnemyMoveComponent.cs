using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshEnemyMoveComponent : EnemyMoveComponent
{
    /// <summary>
    /// Used for pathfinding
    /// </summary>
    protected NavMeshAgent agent;

    // Start is called before the first frame update
    override
    public void Start()
    {
        base.Start();

        // Set move speed
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed * moveSpeedMultiplier;
    }

    /// <summary>
    /// Initiates movement towards given target. 
    /// If enemy is already moving towards a target, sets path towards new target.
    /// </summary>
    /// <param name="target"></param>
    public override void MoveTowards(Vector3 target)
    {
        // Interrupt movement in progress
        if (movement != null)
        {
            StopCoroutine(movement);
        }
        // Move towards target
        movement = StartCoroutine(Approach(target));
    }

    /// <summary>
    /// Tells NavMeshAgent to set a path towards destination
    /// </summary>
    private IEnumerator Approach(Vector3 destination)
    {
        // Set NavMeshAgent towards destination
        agent.SetDestination(destination);

        // Update Animator with current movement speed
        while (!HasReachedDestination()) {
            AnimateMovement(Vector3.Project(agent.desiredVelocity, transform.forward).magnitude);
            yield return null;
        }
        AnimateMovement(0f);

        // Set coroutine to null
        movement = null;
    }

    /// <summary>
    /// Returns true if enemy has stopped at its path's destination
    /// </summary>
    /// <returns></returns>
    public override bool HasReachedDestination()
    {
        if (agent.pathPending) {
            return false;
        }
        else {
            return agent.velocity.magnitude == 0
          && agent.remainingDistance <= agent.stoppingDistance;
        }
    }

    public override void SetMoveSpeedMultiplier(float multiplier)
    {
        base.SetMoveSpeedMultiplier(multiplier);
        // Apply speed multiplier to agent
        agent.speed = moveSpeed * moveSpeedMultiplier;
    }
}

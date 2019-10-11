using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Tactic : MonoBehaviour
{
    protected Enemy enemy;

    // Get references to components
    protected EnemyMoveComponent moveComponent;
    protected EnemyAttackComponent attackComponent;

    /// <summary>
    /// Target the enemy is engaging with (the player)
    /// </summary>
    public Transform target;

    /// <summary>
    /// Current position enemy is moving towards
    /// </summary>
    protected Vector3 destination;

    /// <summary>
    /// How much to correct pathfinding based on the target's movement from destination
    /// (lower value = more frequent correction).
    /// A value of 0 will disable path correction.
    /// </summary>
    [SerializeField]
    protected float pathCorrection;

    /// <summary>
    /// Minimum distance agent will create between player when surrounding
    /// </summary>
    [Range(0, 50)]
    public float minDistanceFromTarget;

    /// <summary>
    /// Max distance agent will create between player when surrounding
    /// </summary>
    [Range(0, 50)]
    public float maxDistanceFromTarget;

    protected Coroutine tacticCoroutine;
    public delegate void OnTacticLoop();

    /// <summary>
    /// Called at the end of each tactic loop
    /// </summary>
    public OnTacticLoop onTacticLoopEnd = delegate {};

    /// <summary>
    /// Executes tactic loop
    /// </summary>
    public void ExecuteTactic()
    {
        if (tacticCoroutine == null) {
            tacticCoroutine = StartCoroutine(TacticLoop());
        }
    }

    /// <summary>
    /// Sends instructions to the MoveComponent/AttackComponent that create the enemy's behavior
    /// </summary>
    protected abstract IEnumerator TacticLoop();

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        moveComponent = enemy.GetMoveComponent();
        attackComponent = enemy.GetAttackComponent();

        // Default to player target
        target = Player.Instance.transform;
    }

    private void Update()
    {
        // TODO: remove this - just a temp measure to enable tactics when player enters room
        if (enemy.active) {
            ExecuteTactic();
        }
    }

    /// <summary>
    /// Returns a position near the target within specified radius (between minDistance and maxDistance)
    /// </summary>
    /// <param name="minDistanceFromTarget">Nearest distance from target</param>
    /// <param name="maxDistanceFromTarget">Furthest distance from target</param>
    protected Vector3 SamplePositionNearTarget(float minDistanceFromTarget, float maxDistanceFromTarget)
    {
        // Choose random position away from player
        Vector3 position = moveComponent.FindPositionNearTarget(target, minDistanceFromTarget, maxDistanceFromTarget);

        // Check if valid navmesh position (currently failing)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, .5f, NavMesh.GetAreaFromName("Walkable"))) {
            position = hit.position;
            ////Debug.Log("Sampled position: " + destination);
        }
        else {
            ////Debug.LogWarning("Sample position failed...");
        }
        return position;
    }

    /// <summary>
    /// Updates destination if distance between target and current destination exceeds correctionThreshold.
    /// Returns true if correction was applied, false otherwise.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="correctionThreshold"></param>
    protected virtual bool ApplyPathCorrection(float correctionThreshold)
    {
        // Update destination if player moved past path correction threshold
        Vector3 targetMovement = target.position - destination;
        if (targetMovement.magnitude > correctionThreshold) {
            Debug.LogFormat("Calculating new position near target...");
            destination = SamplePositionNearTarget(minDistanceFromTarget, maxDistanceFromTarget);

            Debug.LogFormat("Moving towards {0} ({1} units from target)", destination, (target.position - destination).magnitude);
            moveComponent.MoveTowards(destination);
            return true;
        }
        return false;
    }
}

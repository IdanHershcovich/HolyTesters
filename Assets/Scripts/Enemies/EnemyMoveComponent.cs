using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMover))]
public abstract class EnemyMoveComponent : MonoBehaviour
{
    /// <summary>
    /// Movement speed
    /// </summary>
    [SerializeField]
    protected float moveSpeed;

    /// <summary>
    /// Rotation speed (angles per second)
    /// </summary>
    [SerializeField]
    protected float rotateSpeed;


    /// <summary>
    /// Movement speed multiplier, for use with buffs
    /// </summary>
    [SerializeField]
    public float moveSpeedMultiplier = 1f;

    /// The recovery deceleration when in knockback
    /// </summary>
    [SerializeField]
    protected float kbDecel;

    #region Components
    /// <summary>
    /// Used for direct movement
    /// </summary>
    protected ObjectMover objectMover;

    [SerializeField]
    protected Animator animator;
    #endregion

    #region AbstractMethods

    /// <summary>
    /// Initiates movement towards given target. 
    /// If enemy is already moving towards a target, sets path towards new target.
    /// </summary>
    /// <param name="target"></param>
    public abstract void MoveTowards(Vector3 target);

    /// <summary>
    /// Returns true if enemy has reached destination, false otherwise
    /// </summary>
    public abstract bool HasReachedDestination();
    #endregion

    /// <summary>
    /// Coroutine instance used for movement
    /// </summary>
    protected Coroutine movement;

    // Start is called before the first frame update
    public virtual void Start()
    {
        objectMover = GetComponent<ObjectMover>();
    }

    public virtual void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier = multiplier;
    }

    /// <summary>
    /// Update Animator's movement speed
    /// </summary>
    /// <param name="speed"></param>
    protected void AnimateMovement(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    /// <summary>
    /// Stuns and applies a knockback impulse on Enemy with an optional extra stun time
    /// </summary>
    /// <param name="knockback">The impulse (initial velocity) to be applied to the enemy</param>
    /// <param name="extraStunTime">Extra time for Enemy to remain stunned after knockback ends</param>
    public void TakeKnockback(Vector3 knockback, float extraStunTime = 0)
    {
        StartCoroutine(KnockBack(knockback, extraStunTime));
    }

    private IEnumerator KnockBack(Vector3 velocity, float kbStunTimer)
    {
        for (float currSpeed = velocity.magnitude; currSpeed > 0; currSpeed -= kbDecel * Time.fixedDeltaTime) {
            objectMover.Move(velocity.normalized * currSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        for (float timer = 0; timer < kbStunTimer; timer += Time.fixedDeltaTime) {
            yield return new WaitForFixedUpdate();
        }
    }

    #region HelperMethods

    /// <summary>
    /// Returns a valid, ground-level destination near the target that this NavMeshAgent can reach
    /// </summary>
    /// <param name="target"></param>
    /// <param name="minDistance">Min distance of returned position from target</param>
    /// <param name="maxDistance">Max distance of returned position from target</param>
    /// <returns></returns>
    public Vector3 FindPositionNearTarget(Transform target, float minDistance, float maxDistance)
    {
        // Determine distance from target
        float distance = Random.Range(minDistance, maxDistance);

        // Find a random ground-level position at given distance from the target (convert Y-component to Z-component so it stays ground-level)
        Vector3 delta = (Random.insideUnitCircle.normalized * distance);
        Vector3 randomPos = target.position + new Vector3(delta.x, 0, delta.y);

        return randomPos;
    }

    public void RotateTowards(Transform target, bool immediate = true, bool fixedUpdate = false)
    {

        // Get vector towards target
        Vector3 delta = target.position - transform.position;

        if (immediate) 
            {
            // Create rotation towards target
            Vector3 currentAngles = transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.LookRotation(delta, Vector3.up);

            // Only apply the y-component of the rotation (prevents rotation on x/z axes)
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
            transform.rotation = rotation;
        }
        else
        {
            float step = rotateSpeed * ((fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);

            Vector3 newDirection = Vector3.RotateTowards(transform.forward,
                                    Vector3.ProjectOnPlane(delta.normalized, Vector3.up),
                                    step * Mathf.Deg2Rad, 0f);
            transform.forward = Vector3.ProjectOnPlane(newDirection, Vector3.up).normalized;
        }
    }
    #endregion

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
}

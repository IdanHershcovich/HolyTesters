using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    Created by Nick Tang
    4/25/19
*/
public class EnemyNavAgent : MonoBehaviour
{
    GameObject player; //The player object
    NavMeshAgent agent; //The object that will follow the player object
    [SerializeField] private string state;

    [SerializeField]
    private float
        walkSpeed,
        runSpeed;
    
    // for use with Debuffs
    public float speedMultiplier = 1.0f;

    private bool wandering;

    /// <summary> Prevents the enemy from moving if true </summary>
    private bool isStunned;

    /// <summary> reference to the animator </summary>
    Animator animator;

    /// <summary> the coroutine that handles the enemy wander state </summary>
    Coroutine wanderCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = "wander";
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case "target":
                agent.speed = runSpeed * speedMultiplier;
                animator.SetFloat("Speed", agent.speed);
                goToPlayer();
                break;
            case "wander":
                agent.speed = walkSpeed * speedMultiplier;
                animator.SetFloat("Speed", agent.speed);
                if (!wandering)
                    wanderCoroutine = StartCoroutine(Wander());
                break;
        }
    }

    /// <summary>
    /// Handles the enemy wandering state
    /// Makes the enemy wander to one point, wait a bit, then wander to a new point
    /// </summary>
    public IEnumerator Wander()
    {
        wandering = true;
        //animator.SetBool("Idle", false);
        agent.SetDestination(RandomNavmeshLocation(10f));

        //While enemy is moving to new position, max time of 5 seconds, check is state changed
        for (float timer = 0; timer < 5 && transform.position != agent.destination; timer += Time.deltaTime)
        {
            if (state == "wander")
                yield return null;
            else // if state changed
            {
                //end coroutine and let appropriate state be handled
                wandering = false;
                yield break;
            }
        }

        // once enemy reaches new position set to idle

        if (transform.position != agent.destination)
            agent.SetDestination(transform.position);

        animator.SetFloat("Speed", 0.0f);

        // for 5 seconds, have enemy stand in idle
        for (float timer = 0; timer < 5; timer += Time.deltaTime)
        {
            if (state == "wander")
                yield return null;
            else
            {
                wandering = false;
                yield break;
            }
        }

        // after single cycle, break coroutine
        // if state is still in wander, coroutine will be called once more

        wandering = false;
    }

    /// <summary>
    /// informs the navmesh to not move
    /// </summary>
    public void Stun()
    {
        isStunned = true;
        agent.isStopped = true;
    }

    //informs the navmesh to move again
    public void Unstun()
    {
        isStunned = false;
        agent.isStopped = false;
    }

    public void setState(string desiredState) {
        state = desiredState;
    }

    public string getState() {
        return state;
    }

    void goToPlayer() {
        agent.SetDestination(player.transform.position);
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookForPlayerTactic : Tactic
{

    ObjectMover objectMover;
    NavMeshAgent agent;
    private bool foundPlayer;

    [SerializeField]
    private ChargeTactic chargeTactic;
    
    private void Start()
    {
        objectMover = enemy.GetComponent<ObjectMover>();
        agent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override IEnumerator TacticLoop()
    {
        agent.speed = 5;
        agent.angularSpeed = 180;
        agent.isStopped = false;
        enemy.GetAnimator().SetFloat("Speed", 0.0f);

        for (float i = 0; !foundPlayer; i += Time.deltaTime)
        {

            //turning towards player
            enemy.GetMoveComponent().RotateTowards(target, false, true);
            if (i >= 0.3) {
                i = 0;
                agent.SetDestination(Player.Instance.transform.position);
            }
            //if player is infront of enemy
            if (Vector3.Angle(enemy.transform.forward, Vector3.ProjectOnPlane(target.position - enemy.transform.position, Vector3.up).normalized) <= 10)
            {
                //if player is in line of sight
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(new Ray(enemy.transform.position, enemy.transform.forward), out hitInfo, 50, objectMover.layerMask))
                    if (hitInfo.collider.tag == "PlayerRangedHitBox")
                        foundPlayer = true;
            }

            yield return new WaitForFixedUpdate();
        }

        tacticCoroutine = null;

        agent.isStopped = true;
        //Change tactic to Charge
        enemy.SetTactic(chargeTactic);

        onTacticLoopEnd();

        yield return null;
    }
}

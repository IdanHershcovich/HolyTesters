using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTactic : Tactic
{

    ObjectMover objectMover;
    Animator animator;

    private bool hitCollider;
    private bool attacking;

    [SerializeField]
    private DizzyTactic dizzyTactic;

    private void Start()
    {
        animator = enemy.GetAnimator();

        objectMover = enemy.GetComponent<ObjectMover>();
        objectMover.ObjectMoverHit += ObjectMoverHit;
    }

    protected override IEnumerator TacticLoop()
    {
        
        enemy.GetAnimator().SetFloat("Speed", 1.0f);

        while (!hitCollider)
        {

            // Charge towards player
            enemy.GetMoveComponent().RotateTowards(target, false, true);
            destination = enemy.transform.position + (enemy.transform.forward * 5);
            enemy.GetMoveComponent().MoveTowards(destination);

            float distance = Vector3.ProjectOnPlane(enemy.transform.position - Player.Instance.transform.position, Vector3.up).magnitude;
            if (distance < enemy.GetMoveComponent().GetMoveSpeed() && !attacking)
            {
                enemy.GetAnimator().SetTrigger("Attack");
                attacking = true;
            }

            yield return new WaitForFixedUpdate();
        }

        tacticCoroutine = null;
        objectMover.ObjectMoverHit -= ObjectMoverHit;

        //Change tactic to Dizzy
        enemy.SetTactic(dizzyTactic);

        onTacticLoopEnd();

        yield return null;
    }

    private IEnumerator AttackAnimationWait()
    {
        for (float i = 0; i < 2; i += Time.deltaTime)
        {
            yield return null;
        }
        attacking = false;
    }

    private void ObjectMoverHit(ControllerColliderHit hit)
    {

        //Handle in attack
        if (hit.collider.tag == "PlayerRangedHitBox")
        {
            Player.Instance.ReceiveAttack(new HitAttr(30, enemy.transform.forward * 30, 1));
        }

        hitCollider = true;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyTactic : Tactic
{
    [SerializeField]
    private float stunTime = 3;
    [SerializeField]
    private float initialKnockback = 15;

    [SerializeField]
    private Tactic lookForPlayerTactic;

    protected override IEnumerator TacticLoop()
    {
        enemy.GetAnimator().SetTrigger("HitStun");
        enemy.GetAnimator().SetFloat("Speed", 0.0f);
        enemy.TakeKnockback(-transform.forward * initialKnockback);

        for (float i = 0; i < stunTime; i += Time.deltaTime)
        {
            yield return null;
        }

        tacticCoroutine = null;

        //Change tactic to LookForPlayer
        enemy.SetTactic(lookForPlayerTactic);

        onTacticLoopEnd();

        yield return null;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTactic : Tactic
{
    protected override IEnumerator TacticLoop()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            ////Debug.Log("Detected mouse click");

            if (Physics.Raycast(ray, out hit)) {
                enemy.GetMoveComponent().MoveTowards(hit.point);
            }
            // Stop coroutine if invalid position selected
            // TODO: make Tactic.Interrupt() a base class method
            else {
                tacticCoroutine = null;
                yield return null;
            }

            while(!enemy.GetMoveComponent().HasReachedDestination()) {
                ////Debug.Log("Moving...");

                moveComponent.RotateTowards(target);
                yield return null;
            }

            ////Debug.Log("Reached destination");
            tacticCoroutine = null;
            onTacticLoopEnd();
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private Coroutine stepsCoroutine;

    public float stepDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FightMusicTrigger")
        {
            //Debug.Log("Entered Fight Music trigger");
            //SoundController.Instance.SetMusic(SoundType.LEVEL_1);
        }
    }

    private IEnumerator Steps()
    {
        // Alternate between left/right steps (TODO: randomly choose between 3 or more sound effects to add variety)
        while (true) {
            SoundController.Instance.PlaySoundEffect(SoundType.STEP_1);
            yield return new WaitForSeconds(stepDelay);

            SoundController.Instance.PlaySoundEffect(SoundType.STEP_2);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    public void StartSteps()
    {
        stepsCoroutine = StartCoroutine(Steps());
    }


    public void FootStep(int step)
    {
        if (step == 0)
        {
            SoundController.Instance.PlaySoundEffect(SoundType.STEP_1);
        }
        else if (step == 1)
        {
            SoundController.Instance.PlaySoundEffect(SoundType.STEP_2);
        }
    }


    public void EndSteps()
    {
        StopCoroutine(stepsCoroutine);
    }
}

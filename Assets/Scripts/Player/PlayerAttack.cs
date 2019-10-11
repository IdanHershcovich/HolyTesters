using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the attacks of the player
/// </summary>
public class PlayerAttack : MonoBehaviour
{

    /// <summary> Reference to the player animator </summary>
    private Animator animator;
    /// <summary> is the player currently attacking </summary>
    private bool attacking;
    /// <summary> is the slam coolingndown </summary>
    private bool slamCoolingDown;
    /// <summary> Current Attack coroutine </summary>
    Coroutine attackCoroutine;
    /// <summary> Reference to the SoundController </summary>
    private SoundController soundController;
    
    [SerializeField] private List<string> basAttAnimTrigs = new List<string>();
    private int basAttAnimTrigInd = 0;

    private bool canBufferConsecutive;
    private bool consecutiveBuffered;
    private float BaseType = 0.0f;
    [SerializeField] private float consecutiveWait;
    bool coroutineBasicPause;
    bool coroutineSuperPause;

    void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        soundController = SoundController.Instance;
    }

    public void BasicAttack(Grail grail)
    {
        
        if (canBufferConsecutive)
            consecutiveBuffered = true;
        
        // if player successfully attempts to do a light attack and isn't right after a left punch
        if (!attacking)
        {
            ActivateBasicEffect(grail);
        }

    }
    
    public void SuperAttack(Grail grail)
    {
        if (grail.getSuperBarPercent() == 100)// removed !attacking && !slamCoolingDown as a bandaid fix for baseattacks and supers sharing the same cd
        {
            ActivateSuperEffect(grail);
        }
    }

    private void incrementBasAttAnimTrigInd()
    {
        basAttAnimTrigInd++;
        
        if (basAttAnimTrigInd >= basAttAnimTrigs.Count)
        {
            basAttAnimTrigInd = 0;
        }
    }

    private void ActivateBasicEffect(Grail grail)
    {
        animator.SetFloat("BaseType", (float)basAttAnimTrigInd);
        animator.SetTrigger(basAttAnimTrigs[basAttAnimTrigInd]);
        attacking = true;

        incrementBasAttAnimTrigInd();

        coroutineBasicPause = true;
        StartCoroutine(BasicEffect(grail));
    }

    /// <summary>
    /// Set the left punch hit box to active and play SFX
    /// Called via animation event
    /// </summary>
    public void BasicEffectStart()
    {
        coroutineBasicPause = false;
    }

    /// <summary>
    /// Set the left punch hit box to inactive and start left punch cool down
    /// Called via animation event
    /// </summary>
    public void BasicEffectEnd()
    {
        //canBufferConsecutive = true;
        //coroutineBasicPause = false;
    }

    /// <summary>
    /// indicates that the left punch is done, allowing for a right punch to be combo'd in
    /// </summary>
    private IEnumerator BasicEffect(Grail grail)
    {

        while (coroutineBasicPause)
        {
            yield return null;
        }

        grail.baseEffect();

        coroutineBasicPause = true;
        for (float timer = 0; timer < grail.baseAttackLength; timer += Time.deltaTime)
        {
            yield return null;
        }

        canBufferConsecutive = true;
        coroutineBasicPause = false;
        
        float consecutiveWaitStart = 0;
        if (grail.baseCoolDown > consecutiveWait)
        {
            consecutiveBuffered = false;
            canBufferConsecutive = false;
            consecutiveWaitStart = grail.baseCoolDown - consecutiveWait;
        }
        for (float timer = 0; timer < grail.baseCoolDown; timer += Time.deltaTime)
        {

            if (!canBufferConsecutive && consecutiveWaitStart != 0 && timer > consecutiveWaitStart)
                canBufferConsecutive = true;

            yield return null;
        }

        attacking = false;
        canBufferConsecutive = false;

        if (consecutiveBuffered)
        {
            consecutiveBuffered = false;
            BasicAttack(grail);
            yield break;
        }

        for (float timer = 0; timer < consecutiveWait; timer += Time.deltaTime)
        {
            if (attacking)
                yield break;
            else
                yield return null;
        }

        basAttAnimTrigInd = 0;
        
    }

    /// <summary>
    /// Initiate the slam punch animation
    /// </summary>
    private void ActivateSuperEffect(Grail grail)
    {
        animator.SetTrigger("SuperAttack");
        //attacking = true;

        coroutineSuperPause = true;
        StartCoroutine(SuperEffect(grail));

    }

    public void SuperEffectStart()
    {
        coroutineSuperPause = false;
    }

    private IEnumerator SuperEffect(Grail grail)
    {
        /*while (coroutineSuperPause)
        {
            yield return null;
        }
         */
        CameraScript.Instance.StartCameraShake(0.5f, 1.5f, 2.4f);
        grail.superEffect();

        for (float timer = 0; timer < grail.superEndLag; timer += Time.deltaTime)
        {
            yield return null;
        }

        //attacking = false;

    }

    /*
    private IEnumerator _ProcessShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        Noise(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        cmCam.topRig.Noise.m_AmplitudeGain = amplitudeGain;
        cmCam.middleRig.Noise.m_AmplitudeGain = amplitudeGain;
        cmCam.bottomRig.Noise.m_AmplitudeGain = amplitudeGain;

        cmCam.topRig.Noise.m_FrequencyGain = frequencyGain;
        cmCam.middleRig.Noise.m_FrequencyGain = frequencyGain;
        cmCam.bottomRig.Noise.m_FrequencyGain = frequencyGain;

    }
    */
}

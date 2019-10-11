using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing animation events for Templar/Player
/// author: Michelle Zhong
/// </summary>
public class PlayerAnimEvents : MonoBehaviour
{

    /// <summary> The script holding the data and functions to work with the player's attacks </summary>
    [SerializeField]
    private PlayerAttack attackScript;

    private void Start()
    {
        attackScript = transform.parent.GetComponentInChildren<PlayerAttack>();
    }

    /// <summary>
    /// Turn on the hitbox for the player slam attack
    /// </summary>
    public void TurnSuperCollisionOn()
    {
        attackScript.SuperEffectStart();
    }

    /// <summary>
    /// Turn on the hitbox for the player basic attack
    /// </summary>
    public void TurnBasicCollisionOn()
    {
        attackScript.BasicEffectStart();

    }

    /// <summary>
    /// Turn off the hitbox for the player basic attack
    /// </summary>
    public void TurnBasicCollisionOff()
    {
        attackScript.BasicEffectEnd();
    }


    /// <summary>
    /// Animation Event to play footstep sounds
    /// </summary>
    /// <param name="step">The step in the animation (also which sound to play)</param>
    public void FootSteps(int step)
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
}

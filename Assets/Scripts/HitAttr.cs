using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct holding info of an attack and how it effects the opposing character
/// </summary>
[System.Serializable]
public struct HitAttr
{
    /// <summary>
    /// Damage to the opposing character
    /// </summary>
    public float damage;

    /// <summary>
    /// Knockback impulse applied
    /// </summary>
    public Vector3 knockback;

    /// <summary>
    /// The time the opposing character remains stunned after knockback finishes
    /// </summary>
    public float stunTime;

    /// <summary>
    /// The time the opposing character has iframes for, -1 to use opposing character's default iframe time
    /// </summary>
    public float iFrameTimeMod;

    /// <summary>
    /// Initializes hitAtt 
    /// </summary>
    /// <param name="dmg">The attacks damage</param>
    /// <param name="kb">The knockback in initial velocity</param>
    /// <param name="stun">The stun time after knockback ends</param>
    /// <param name="iFrameTime">Modifier for iFrame time, use -1 to use opposing character's default iFrameTime</param>
    public HitAttr(float dmg, Vector3 kb, float stun, float iFrameTime=-1)
    {

        damage = dmg;
        knockback = kb;
        stunTime = stun;
        iFrameTimeMod = iFrameTime;
    }


    /// <summary>
    /// Initializes hitAtt with no stun time
    /// </summary>
    /// <param name="dmg">The attacks damage</param>
    /// <param name="kb">The knockback in initial velocity</param>
    public HitAttr(float dmg, Vector3 kb)
    {
        damage = dmg;
        knockback = kb;
        stunTime = 0;
        iFrameTimeMod = -1;

    }
    /// <summary>
    /// Initializes hitAtt with no stun time or knockback
    /// </summary>
    /// <param name="dmg">The attacks damage</param>
    public HitAttr(float dmg,float iFrameTime=-1)
    {
        damage = dmg;
        knockback = Vector3.zero;
        stunTime = 0;
        iFrameTimeMod = -1;

    }
}

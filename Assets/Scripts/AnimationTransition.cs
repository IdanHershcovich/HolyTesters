using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition : MonoBehaviour
{

    public enum SuperAnimationType
    {
        Slam,
    }

    public static float SuperAnimToFloat(SuperAnimationType type)
    {
        switch (type)
        {
            case SuperAnimationType.Slam:
                {
                    return 0.0f;
                }
            default:
                {
                    return 0.0f;
                }
        }
    }
}

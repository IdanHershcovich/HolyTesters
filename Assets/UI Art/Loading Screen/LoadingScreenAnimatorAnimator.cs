using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenAnimatorAnimator : MonoBehaviour
{
    [SerializeField]
    private Texture[] screens;

    public int current;
    public bool directionRight = true;
    private RawImage ri;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponent<RawImage>();
        ri.texture = screens[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer % screens.Length > current)
        {
            ri.texture = screens[current];
            if (directionRight)
            {
                current += 1;
            }
            else
            {
                current -= 1; 
            }
            if (current == 0 || current == screens.Length - 1)
            {
                directionRight = !directionRight;
            }
        }

    }
}

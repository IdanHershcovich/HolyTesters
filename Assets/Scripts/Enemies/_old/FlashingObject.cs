using UnityEngine;
using System.Collections;

public class FlashingObject : MonoBehaviour
{

    private Material mat;
    private Color[] colors = { Color.white, Color.red };

    public void Awake()
    {

        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;

    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Flash(2f, 0.05f));
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            mat.color = colors[index % 2];

            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
    }

}
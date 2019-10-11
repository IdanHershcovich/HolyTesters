using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireSlash : MonoBehaviour
{

    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform middlePoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private float duration;

    [SerializeField]
    private TrailRenderer mainRenderer;
    [SerializeField]
    private TrailRenderer additiveRenderer;

    public void Slash()
    {
        StartCoroutine(SlashCoroutine());
    }

    private IEnumerator SlashCoroutine()
    {
        mainRenderer.enabled = true;
        additiveRenderer.enabled = true;
        Vector3 globStart = new Vector3(startPoint.position.x, startPoint.position.y, startPoint.position.z);
        Vector3 globMiddle = new Vector3(middlePoint.position.x, middlePoint.position.y, middlePoint.position.z);
        Vector3 globEnd = new Vector3(endPoint.position.x, endPoint.position.y, endPoint.position.z);

        for (float timer = 0f; timer < duration; timer += Time.deltaTime)
        {
            Vector3 m1 = Vector3.Lerp(globStart, globMiddle, timer / duration);
            Vector3 m2 = Vector3.Lerp(globMiddle, globEnd, timer / duration);
            transform.position = Vector3.Lerp(m1, m2, timer / duration);
            yield return null;
        }

        for (float timer = 0f; timer < duration; timer += Time.deltaTime)
        {
            yield return null;
        }

        mainRenderer.enabled = false;
        additiveRenderer.enabled = false;
        transform.localPosition = Vector3.zero;
    }
}

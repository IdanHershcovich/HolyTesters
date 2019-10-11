using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private float arcLength = 1.0f;
    [SerializeField]
    private float arcLengthDeviation = 1.0f;
    [SerializeField]
    private float arcAngleDeviation = 0.5f;
    [SerializeField]
    private float duration = 0.50f;
    private float timer;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private Vector3 Randomize(Vector3 newVector, float devation)
    {
        newVector += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * devation;
        newVector.Normalize();
        return newVector;
    }

    /// <summary>
    /// Creates a lightning effect chaining between two transforms.
    /// </summary>
    public void Zap(Transform source, Transform target)
    {
        StartCoroutine(ZapCoroutine(source, target));
    }

    public void Zap(Transform source, Vector3 target)
    {
        StartCoroutine(ZapCoroutine(source, target));
    }

    private IEnumerator ZapCoroutine(Transform source, Transform target)
    {
        for (timer = duration; timer > 0; timer -= Time.deltaTime)
        {
            //Check to make sure transforms are not null in case the thing being zapped has been destroyed
            if (source != null && target != null)
            {
                //Create a vector3 to hold our point as it moves along the line and initilize it to the source position
                Vector3 iterativePoint = source.position;
                int i = 1;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, source.position);

                //Loop through creating points along line (with randomization) until distance has been reached
                while (Vector3.Distance(target.position, iterativePoint) > arcLength)
                {
                    //Make a new position
                    lineRenderer.positionCount = i + 1;

                    //Start with a vector that points from the last point to the target and then randomize it a bit
                    Vector3 newPoint = target.position - iterativePoint;
                    newPoint.Normalize();
                    newPoint = Randomize(newPoint, arcAngleDeviation);
                    newPoint *= Random.Range(arcLength * arcLengthDeviation, arcLength);
                    newPoint += iterativePoint;
                    lineRenderer.SetPosition(i, newPoint);

                    //Iterate
                    i++;
                    iterativePoint = newPoint;
                }

                //Connect it to the target
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, target.position);

                yield return null;
            }
            else
            {
                break;
            }
        }
        
        //Reset line renderer
        lineRenderer.positionCount = 1;
    }

    private IEnumerator ZapCoroutine(Transform source, Vector3 target)
    {
        for (timer = duration; timer > 0; timer -= Time.deltaTime)
        {
            //Check to make sure transforms are not null in case the thing being zapped has been destroyed
            if (source != null && target != null)
            {
                //Create a vector3 to hold our point as it moves along the line and initilize it to the source position
                Vector3 iterativePoint = source.position;
                int i = 1;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, source.position);

                //Loop through creating points along line (with randomization) until distance has been reached
                while (Vector3.Distance(target, iterativePoint) > arcLength)
                {
                    //Make a new position
                    lineRenderer.positionCount = i + 1;

                    //Start with a vector that points from the last point to the target and then randomize it a bit
                    Vector3 newPoint = target - iterativePoint;
                    newPoint.Normalize();
                    newPoint = Randomize(newPoint, arcAngleDeviation);
                    newPoint *= Random.Range(arcLength * arcLengthDeviation, arcLength);
                    newPoint += iterativePoint;
                    lineRenderer.SetPosition(i, newPoint);

                    //Iterate
                    i++;
                    iterativePoint = newPoint;
                }

                //Connect it to the target
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, target);

                yield return null;
            }
            else
            {
                break;
            }
        }

        //Reset line renderer
        lineRenderer.positionCount = 1;
    }
}
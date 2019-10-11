using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFix : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        ParticleSystem particle;
    }

    private void Start()
    {
        trailRenderer.enabled = true;
        trailRenderer.Clear();
    }

    private void OnDisable()
    {
        trailRenderer.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Asmodeus_Cutscene_Trigger : MonoBehaviour
{
    public GameObject levelbuilder;
    bool isPlaying;
    public PlayableDirector cutscene;

    void Start()
    {
        levelbuilder = GameObject.FindGameObjectWithTag("LevelBuilder");
        this.transform.parent = null;
        cutscene.transform.parent = null;
        isPlaying = false;
    }
    //Add skip cutscene button
    void Update()
    {
        if (Input.GetKeyDown("o")) {
            cutscene.Stop();
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isPlaying)
        {
            cutscene.gameObject.SetActive(true);
            isPlaying = true;
            levelbuilder.SetActive(false);
            cutscene.Play();
        }
    }

    void OnEnable()
    {
        cutscene.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector) {
        if (cutscene == aDirector) {
            isPlaying = true;
            cutscene.gameObject.SetActive(false);
            levelbuilder.SetActive(true);
        }
    }

    void OnDisable()
    {
        cutscene.stopped -= OnPlayableDirectorStopped;
    }
}

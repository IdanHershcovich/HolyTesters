using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem deathParticleSystem;

    private GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {

        gameOverScreen = Player.Instance.GetComponent<PlayerStatus>().gameOverScreen;

        SoundController.Instance.PlaySoundEffect(SoundType.PLAYER_DEATH);

        try
        {
            // Instantiate/play particle system
            ParticleSystem psystem = Instantiate(deathParticleSystem, transform.position, Quaternion.Euler(Vector3.up));
            psystem.Play();

            // Destroy after 2 seconds

            Destroy(psystem.gameObject, 2);
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError(ex.ToString());
            return;
        }

        StartCoroutine(GameOver(5, 2));

    }
    //Wait coroutine for scene change
    IEnumerator GameOver(int screenLength, int delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.SetActive(true);

        yield return new WaitForSeconds(screenLength);
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

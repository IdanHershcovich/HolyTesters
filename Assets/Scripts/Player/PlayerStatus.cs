using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
    Created by Kat Leitao
    4/30/19
*/

public class PlayerStatus : MonoBehaviour
{
    private float _health; //Player's current health
    public float health
    {
        get
        {
            return _health;
        }

        private set
        {
            _health = value;

            if (health < 0)
                _health = 0;
            else if (health > maxHealth)
                _health = maxHealth;

            healthbar.value = health;

            if (health == 0)
                Die();
        }
    }

    [SerializeField] private float maxHealth; //Player's max health
    [SerializeField] private Slider healthbar; // UI element for healthbar
    [SerializeField] private float defaultIFrameTime;

    [SerializeField]
    private GameObject deathObject;

    private bool hasIFrames;

    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Awake()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = maxHealth;

        health = maxHealth;
    }
    
    public float GetMaxHealth(){
        return maxHealth;
    }

    //Function to call to inflict damage to the player
    public void TakeDamage(float amount, float iFrameTimeMod=-1)
    {
        if (hasIFrames || amount <= 0)
            return;

        health -= amount;
        StartCoroutine(StartIFrames(iFrameTimeMod));

        // Play sound effect
        SoundController.Instance.PlaySoundEffect(SoundType.TAKE_DAMAGE);
    }

    public void Heal(float healBy)
    {
        if (healBy <= 0)
            return;

        health += healBy;
    }

    public void FullHeal()
    {
        health = maxHealth;
    }

    private IEnumerator StartIFrames(float iFrameTimeMod=-1)
    {
        if (hasIFrames)
            yield break;

        SkinnedMeshRenderer meshRend = GetComponentInChildren<SkinnedMeshRenderer>();
        meshRend.material.color = Color.red/2;

        hasIFrames = true;

        float j = 0; ;
        for (float i = 0; i < ((iFrameTimeMod == -1) ? defaultIFrameTime : iFrameTimeMod); i += Time.deltaTime)
        {
            if (j >= .2)
            {
                meshRend.enabled = !meshRend.enabled;
                j = 0;
            }
            yield return null;
            j += Time.deltaTime;
        }
        meshRend.material.color = Color.white;
        meshRend.enabled = true;

        hasIFrames = false;
    }

    //Opens the gameover screen and main-menus in 5 seconds.
    private void Die()
    {
        Instantiate(deathObject, transform.position, transform.rotation);
        Player.Instance.Die();
        Destroy(gameObject);
    }
}
    
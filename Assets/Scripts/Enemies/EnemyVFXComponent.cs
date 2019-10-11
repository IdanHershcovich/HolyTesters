using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXComponent : MonoBehaviour
{
    /// <summary>
    /// Reference to enemy
    /// </summary>
    protected Enemy enemy;

    /// <summary>
    /// Particle effect played on enemy death (reference to prefab)
    /// </summary>
    [SerializeField]
    protected ParticleSystem deathParticleSystem;

    /// <summary>
    /// Particle effect for enemy spawn (reference to prefab)
    /// </summary>
    [SerializeField]
    protected ParticleSystem spawnParticleSystem;

    /// <summary>
    /// Instantiates & plays particle system, then destroys it after lifetime
    /// </summary>
    /// <param name="psystem">Particle effect to play</param>
    /// <param name="lifetime">Time to wait before destroying it</param>
    private void PlayParticleEffect(ParticleSystem psystem, float lifetime)
    {
        try {
            // Get reference to enemy if needed
            if(enemy == null) {
                enemy = GetComponent<Enemy>();
            }
            //Debug.LogFormat("Playing {0} particle effect for {1}", psystem.name, enemy.name);

            // Instantiate/play particle system
            psystem = Instantiate(psystem, enemy.transform.position, Quaternion.Euler(Vector3.up));
            psystem.Play();

            // Destroy after 2 seconds
            // TODO: get the actual lifetime from the psystem itself
            Destroy(psystem.gameObject, lifetime);
        }
        catch(NullReferenceException ex) {
            Debug.LogError(ex.ToString());
            return;
        }
    }

    /// <summary>
    /// Instantiates/plays the death particle effect
    /// </summary>
    public void PlayDeathVFX()
    {
        PlayParticleEffect(deathParticleSystem, 2f);
    }

    /// <summary>
    /// Plays the enemy spawn particle effect
    /// </summary>
    public void PlaySpawnVFX()
    {
        PlayParticleEffect(spawnParticleSystem, 2f);
    }
}

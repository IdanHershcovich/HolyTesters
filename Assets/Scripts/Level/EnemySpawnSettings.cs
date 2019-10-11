using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains settings for enemies to be populated in a Room (how many, what types, etc)
/// </summary>
[System.Serializable]
public class EnemySpawnSettings
{
    /// <summary>
    /// Container objects whose children are the enemy spawn positions 
    /// </summary>
    public List<Transform> enemySpawnPositionContainers;

    public EnemySettings enemySettings;

    /// <summary>
    /// Returns list of enemy spawn positions
    /// </summary>
    public List<Transform> GetEnemySpawnPositions()
    {
        List<Transform> enemySpawnPositions = new List<Transform>();

        // Iterate over each container
        foreach (Transform container in enemySpawnPositionContainers) {
            // Add each child (spawn position) to list
            foreach (Transform spawnPosition in container) {
                enemySpawnPositions.Add(spawnPosition);
            }
        }
        return enemySpawnPositions;
    }
}
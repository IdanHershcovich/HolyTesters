using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemySettings")]
public class EnemySettings : ScriptableObject
{

    public int minNumberOfEnemies;
    public int maxNumberOfEnemies;

    /// <summary>
    /// Used for spawn settings specific to one type of enemy
    /// </summary>
    [System.Serializable]
    public class EnemyParams
    {
        public Enemy enemy;

        /// <summary>
        /// Maximum amount of this enemy to spawn
        /// </summary>
        public int maxNumber;
    }
    public List<EnemyParams> EnemyTypes;
}

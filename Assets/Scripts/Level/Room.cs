using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DoorLocation
{
    N,
    S,
    E,
    W,
}

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public DoorLocation[] doorDirections; //coordinates in the SAME order as the doorways
    public MeshCollider MeshCollider;
    //added by Rush 7/28 for grail pedestal to only appear in grail rooms
    public GameObject grailPedestal;
    //added by Rush 7/29 for grail spawn effect
    public ParticleSystem spawnParticle;
    public int[] coordinates = { 0, 0 }; //the coordinates of the room in the level builder
    public int rotation = 1; //1 for normal, 2 for 90, 3 for 180, 4 for 270

    /// <summary>
    /// List of enemies spawned in room
    /// </summary>
    protected List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> Enemies { get { return enemies; } }


    public GameObject HideMe; //GameObject to hide unnecessary assets for level generation -Michelle Zhong

    //Added by Rush, 7/3/19.
    //Added to know which grails are still available, and to communicate with the randomization script.
    public Transform grailSpawnLocation;
    public GrailRandomizer grailRandomizer;

    public Bounds RoomBounds
    {
        get { return MeshCollider.bounds; }
        
    }

    [Header("Enemies")]
    /// <summary>
    /// Controls amount/types of enemies that spawn in this room
    /// </summary>
    public EnemySpawnSettings enemySpawnSettings;

    public void Start()
    {
        //grailPedestal.SetActive(false);
    }

    /// <summary>
    /// Returns true if this room should be populated with enemies, false otherwise
    /// </summary>
    /// <returns></returns>
    public bool WillSpawnEnemies()
    {
        if (enemySpawnSettings.enemySettings == null)
            return false;
        else
            return enemySpawnSettings.enemySettings.maxNumberOfEnemies > 0;
    }

    /// <summary>
    /// Makes enemies appear in room (must be called after Populate())
    /// </summary>
    public void SpawnEnemies()
    {
        if(!WillSpawnEnemies()) {
            return;
        }

        // Set each enemy to active
        foreach(Enemy enemy in enemies) {
            enemy.gameObject.SetActive(true);
            // TODO: when we have a combat manager, initialize it with these enemies
        }
    }

    public virtual void Populate()
    {
        PopulateEnemies();
    }

    /// <summary>
    /// Instantiates enemies in the room after level generation 
    /// (enemies don't appaer until SpawnEnemies() is called).
    /// </summary>
    private void PopulateEnemies()
    {
        if (!WillSpawnEnemies())
        {
            return;
        }

        // Track occupied/unoccupied spawn locations (so we don't spawn multiple enemies in one place)
        List<Transform> availableSpawnPositions = enemySpawnSettings.GetEnemySpawnPositions();

        // Track how many of each enemy type has been spawned
        Dictionary<Enemy, int> enemySpawnCounts = new Dictionary<Enemy, int>();

        // Determine number of enemies to populate the room with
        int numEnemies = Random.Range(enemySpawnSettings.enemySettings.minNumberOfEnemies, enemySpawnSettings.enemySettings.maxNumberOfEnemies + 1);
        for (int i = 0; i < numEnemies; i++)
        {
            try
            {
                // Select type of enemy to spawn
                Enemy enemy = SelectEnemyType(ref enemySpawnCounts);
                if (enemy == null)
                {
                    Debug.LogWarning("Couldn't select a proper enemy...");
                }

                // Select spawn position
                int spawnPosIndex = Random.Range(0, availableSpawnPositions.Count);
                Transform spawnLocation = availableSpawnPositions[spawnPosIndex];

                // Spawn at selected location
                CreateEnemy(enemy, spawnLocation);

                // Remove spawn location from list (so we don't spawn another enemy there)
                availableSpawnPositions.RemoveAt(spawnPosIndex);
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogErrorFormat("NullReferenceException in room {0} while spawning enemy {1}", name, i);
                continue;
            }
        }
    }

    /// <summary>
    /// Selects enemy type to be spawned (updates dictionary enemySpawnCounts accordingly)
    /// If all enemies have exceeded max spawn counts, returns random enemy.
    /// </summary>
    /// <param name="enemySpawnCounts"></param>
    /// <returns></returns>
    private Enemy SelectEnemyType(ref Dictionary<Enemy, int> enemySpawnCounts)
    {
        // Get list of enemies to choose from
        EnemySettings enemySettings = enemySpawnSettings.enemySettings;
        List<Enemy> enemyChoices = enemySpawnSettings.enemySettings.EnemyTypes.Select(i => i.enemy).ToList();

        Enemy enemy = null;
        // Loop until we select a valid enemy (if none found, selects one at random)
        for(int i = 0; i < enemySettings.EnemyTypes.Count; i++) {
            int enemyIndex = Random.Range(0, enemyChoices.Count);
            enemy = enemyChoices[enemyIndex];

            // Get max amount to be spawned
            int maxAmount = enemySettings.EnemyTypes.Find(e => e.enemy == enemy).maxNumber;

            // Add to dictionary if it's our first time spawning this enemy
            if (!enemySpawnCounts.ContainsKey(enemy)) {
                enemySpawnCounts.Add(enemy, 0);
            }

            // Select this enemy if we haven't already spawned the maximum amount
            int amountSpawned = enemySpawnCounts[enemy];
            if (amountSpawned < maxAmount) {
                break;
            }

            // Otherwise, skip this option and iterate over the rest
            else {
                enemyChoices.Remove(enemy);
            }
        }

        // Increment spawn count and return selected enemy
        enemySpawnCounts[enemy]++;
        return enemy;
    }

    private void CreateEnemy(Enemy enemy, Transform spawnLocation)
    {
        // Spawn selected enemy at selected location and add to list
        Enemy enemyInstance = Instantiate(enemy, spawnLocation.position, spawnLocation.rotation, transform) as Enemy;
        enemies.Add(enemyInstance);

        // Set enemy to inactive until player enters room
        enemyInstance.gameObject.SetActive(false);
    }

    /// <author>
    /// Michelle Zhong
    /// </author>
    /// <date>
    /// 7/3/2019
    /// </date>
    /// <summary>
    /// Activate Room assets hidden by the HideMe gameObject
    /// </summary>
    public void ShowRoom()
    {
        HideMe.SetActive(true);
    }
    /// <author>
    /// Michelle Zhong
    /// </author>
    /// <date>
    /// 7/3/2019
    /// </date>
    /// <summary>
    /// Hide Room assets hidden by the HideMe gameObject 
    /// </summary>
    public void HideRoom()
    {
        HideMe.SetActive(false);
    }
    //Spawn a randomized grail. Can now duplicate, must change later.
    public void SpawnGrail()
    {
        grailRandomizer.OrganizeSpawnableGrails();
        SoundController.Instance.PlaySoundEffect(SoundType.GRAIL_SPAWN);
        spawnParticle.Play();
        int index = Random.Range(0, grailRandomizer.spawnOptions.Count);
        Grail g = Instantiate(grailRandomizer.spawnOptions[index], grailSpawnLocation);
        //need to trim name for it to be picked up correctly, so the code is searching by name somewhere.
        g.name = g.name.Replace("(Clone)", "").Trim();
    }

    /// <summary>
    /// Return the list of doorway directions/locations based on the room rotation
    /// </summary>
    /// <returns></returns>
    public DoorLocation[] GetRotatedDoors()
    {
        DoorLocation[] rotated = new DoorLocation[doorDirections.Length];
        if (rotation == 1)
        {
            return doorDirections;
        }
        if (rotation == 2) //90 degrees
        {
            for (int i = 0; i < doorDirections.Length; i++)
            {
                if(doorDirections[i] == DoorLocation.N)
                {
                    rotated[i] = DoorLocation.E;
                }
                else if(doorDirections[i] == DoorLocation.E)
                {
                    rotated[i] = DoorLocation.S;
                }
                else if (doorDirections[i] == DoorLocation.S)
                {
                    rotated[i] = DoorLocation.W;
                }
                else //if (doorDirections[i] == DoorLocation.W)
                {
                    rotated[i] = DoorLocation.N;
                }
            }
        }
        else if (rotation == 3) //180 degrees
        {
            for (int i = 0; i < doorDirections.Length; i++)
            {
                if (doorDirections[i] == DoorLocation.N)
                {
                    rotated[i] = DoorLocation.S;
                }
                else if (doorDirections[i] == DoorLocation.E)
                {
                    rotated[i] = DoorLocation.W;
                }
                else if (doorDirections[i] == DoorLocation.S)
                {
                    rotated[i] = DoorLocation.N;
                }
                else //if (doorDirections[i] == DoorLocation.W)
                {
                    rotated[i] = DoorLocation.E;
                }
            }
        }
        else if (rotation == 4) //270 degrees
        {
            for (int i = 0; i < doorDirections.Length; i++)
            {
                if (doorDirections[i] == DoorLocation.N)
                {
                    rotated[i] = DoorLocation.W;
                }
                else if (doorDirections[i] == DoorLocation.E)
                {
                    rotated[i] = DoorLocation.N;
                }
                else if (doorDirections[i] == DoorLocation.S)
                {
                    rotated[i] = DoorLocation.E;
                }
                else //if (doorDirections[i] == DoorLocation.W)
                {
                    rotated[i] = DoorLocation.S;
                }
            }
        }
        return rotated;
    }

    /// <summary>
    /// Get the Cardinal Direction of a door as well as the Doorway
    /// </summary>
    /// <returns></returns>
    public List<(DoorLocation, Doorway)> getDoorways()
    {
        List<(DoorLocation, Doorway)> doors = new List<(DoorLocation, Doorway)>();
        DoorLocation[] adjustedDirections = GetRotatedDoors();
        for(int i = 0; i < doorways.Length; i++)
        {
            doors.Add((adjustedDirections[i], doorways[i]));
        }
        return doors;
    }
}

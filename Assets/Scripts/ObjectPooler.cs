using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ObjectPoolItem
{
  public int amountToPool; //amount of GameObjects to make
  public GameObject objectToPool; //GameObject to pool
  public bool shouldExpand; //whether we can Intantiate more of object to pool
}


/// <summary>
/// ObjectPooler class from 
/// https://www.raywenderlich.com/847-object-pooling-in-unity
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance; // Singleton
    public List<ObjectPoolItem> itemsToPool; // List of GameObjects to intantiate
    public List<GameObject> pooledObjects;
    // Start is called before the first frame update
    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SharedInstance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneWasChanged;
            SceneManager.activeSceneChanged += OnSceneWasSwitched;
        }
    }

    void OnSceneWasSwitched(Scene scene, Scene newscene)
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool, this.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    public void OnSceneWasChanged(Scene scene, LoadSceneMode mode)
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool, this.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

    }

    void Destroy()
    {
        SceneManager.activeSceneChanged -= OnSceneWasSwitched;
        SceneManager.sceneLoaded -= OnSceneWasChanged;
    }

    /// <summary>
    /// Get a Pooled Object with tag.  To return to the pool set the returned gameObject to inActive
    /// </summary>
    /// <param name="tag"> tag is the tag associated with the Pooled object you want to get</param>
    /// <returns>Pooled object with input tag</returns>
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, this.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

}

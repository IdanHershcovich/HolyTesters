using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneObjectTransition : MonoBehaviour
{
    public List<Transform> respawnPoints;

    [SerializeField]
    public UnityEvent spawn;

    [SerializeField]
    int levelIndex;

    [SerializeField]
    List<GameObject> toBeMoved;


    [SerializeField]
    List<GameObject> toBeDestroyed;

    bool isLoading;

    void Start()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            levelIndex = 0;
        spawn.AddListener(delegate { turnOn(); });
        isLoading = true;
        toBeMoved.Add(Camera.main.gameObject);
        toBeMoved.Add(Player.Instance.gameObject);
        Camera[] cameras = Camera.allCameras;
        foreach (Camera cam in cameras)
        {
            if (cam.tag == "Follow_Cam")
            {
                toBeMoved.Add(cam.gameObject);
            }
        }
        toBeMoved.Add(UI_Player_Canvas.Instance.gameObject);
        toBeMoved.Add(UI_Inventory.Instance.gameObject);
        toBeMoved.Add(GameObject.FindGameObjectWithTag("InputController"));
        toBeMoved.Add(GameObject.FindGameObjectWithTag("Follow_Cam"));

        // Destroy sound controller (otherwise instance in next scene will destroy self)
        toBeDestroyed.Add(SoundController.Instance.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLoading)
        {
            StartCoroutine(LoadYourAsyncScene());
            isLoading = false;
        }

    }

    IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // Before loading next scene, destroy specified objects
        toBeDestroyed.ForEach(obj => Destroy(obj));
        yield return null;

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);


        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        for (int i = 0; i < toBeMoved.Count; i++)
        {
            toBeMoved[i].transform.parent = null;
            SceneManager.MoveGameObjectToScene(toBeMoved[i], SceneManager.GetSceneByBuildIndex(levelIndex));
        }

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);

    }

    public void turnOn() {
        this.gameObject.SetActive(true);
    }

}

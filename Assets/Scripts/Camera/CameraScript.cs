using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Handles the basic movement of the camera when transitioning from one room to the next
/// </summary>
public class CameraScript : Singleton<CameraScript>
{ 
    /// <summary> The center of the current room at which the camera is based around </summary>
    Vector3 center;

    /// <summary> The distance from the center </summary>
    [SerializeField] private float distance;
    /// <summary> The height from the floor </summary>
    [SerializeField] private float height;
    /// <summary> The angle at which the camera looks down </summary>
    [SerializeField] private float angle;
    /// <summary> the speed of the transition from room to room </summary>
    [SerializeField] private float speed;
    
    /// <summary> The initial room transitioner object used to find the initial center at the start room </summary>
    [SerializeField] GameObject initRoomTransitioner;

    /// <summary> The cinemachine brain associated with this camera.</summary>
    private CinemachineBrain cinemachineBrain;
    /// <summary> The virtual camera associated with this camera.</summary>
    private CinemachineVirtualCamera virtualCamera;
    /// <summary> The NoiseComponent associated with the Cinemachine Brain for this camera.</summary>
    private CinemachineBasicMultiChannelPerlin cinemachineCameraNoise;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get components
        cinemachineBrain = GetComponent<CinemachineBrain>();
        virtualCamera = GameObject.FindGameObjectWithTag("Follow_Cam").GetComponent<CinemachineVirtualCamera>();
        cinemachineCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //get initial center and set camera transform based on that
        center = initRoomTransitioner.transform.position + Vector3.forward * 5; // adds to distance since for some reason only when starting, distance is off
        transform.rotation = Quaternion.Euler(angle, 0, 0);
        transform.position = new Vector3(center.x, height, center.z - distance);


        initRoomTransitioner.SetActive(false); //transitioner center is inaccurate at start but is accurate after changing rooms once, unknown why, but this prevents the camera from changing wrongly when still in the start room
    }

    /// <summary>
    /// Changes center that camera is focused on to new center
    /// </summary>
    /// <param name="_center"> the new center</param>
    public void ChangeCenter(Vector3 _center)
    {
        //sets the new center
        center = _center;

        //see last line in start function, this sets it back to active after changing rooms
        /*
        if (!initRoomTransitioner.activeSelf)
        {
            initRoomTransitioner.SetActive(true);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //moves the camera to its current set position based on the current center
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(center.x, height, center.z - distance), speed * Time.deltaTime);
    }

    /// <summary>
    /// Shakes the camera for a specified duration. Calls a coroutine on CameraScriptObject.
    /// </summary>
    /// <param name="duration">Duration to shake the camera for.</param>
    /// <param name="amplitudeGain">How large each shake can be.</param>
    /// <param name="frequencyGain">How fast each shake can be.</param>
    /// <returns></returns>
    public void StartCameraShake(float duration, float amplitudeGain, float frequencyGain)
    {
        StartCoroutine(CameraShake(duration, amplitudeGain, frequencyGain));
    }
    
    private IEnumerator CameraShake(float duration, float amplitudeGain, float frequencyGain)
    {
        //Get current live camera and noise component from camera
        CinemachineVirtualCamera virtualCamera = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.m_AmplitudeGain = amplitudeGain;
        virtualCameraNoise.m_FrequencyGain = frequencyGain;

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return null;

        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = 0;
    }
}

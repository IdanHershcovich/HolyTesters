using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

/// <summary>
/// Controls the input of the game through use of enum type and switch statements
/// Based on current InputState, input is handled differently
/// </summary>
public class InputController : MonoBehaviour
{
    
    /// <summary> enum type used to keep track of how the input from user should be handled </summary>
    private enum InputState { MENU, GAMEPLAY, PAUSE }
    /// <summary> The current state of how input should be handled </summary>
    private InputState currentState;


    Plane xzPlane = new Plane(Vector3.up, Vector3.zero);

    private Vector3 prevMousePos;
    private bool mouseMoved { get { return Input.mousePosition != prevMousePos; } }
    private bool trackMousePosition;

    /// <summary> ReWired Player object </summary>
    private Rewired.Player rwplayer;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ReWired
        rwplayer = ReInput.players.GetPlayer(0);

        // Initialize state
        currentState = InputState.GAMEPLAY;

        prevMousePos = Input.mousePosition;
        trackMousePosition = false;

    }


    /// <summary>
    /// Gets the new facing direction for the player mased on current Mouse Position
    /// </summary>
    /// <returns>The normalized vector 3 representing the new facing direction of the player</returns>
    public Vector3 GetPlayerFacingDirection()
    {

        Vector2 mousePos = Input.mousePosition;
        Vector3 mousePosOnCam = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        Vector3 mousePosOnPlane;

        Ray ray;

        if (Camera.main.orthographic)
            ray = new Ray(mousePosOnCam, Camera.main.transform.forward);
        else
            ray = new Ray(mousePosOnCam, Camera.main.ScreenPointToRay(mousePos).direction);

        float enter = 0; // raycast hit distance
        if (xzPlane.Raycast(ray, out enter))
        {
            mousePosOnPlane = ray.GetPoint(enter);
        }
        else
        {
            mousePosOnPlane = Vector3.zero;
        }

        if (mousePosOnPlane == Vector3.zero)
            return Vector3.zero;
        else
            return Vector3.ProjectOnPlane(mousePosOnPlane - Player.Instance.transform.position, xzPlane.normal).normalized;
    }

    /// <summary>
    /// Runs the frame input intended while in the gameplay InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunGameplayFrameInput()
    {
        // For debugging, should be removed and locked to a restricted developer mode
        if (rwplayer.GetButtonDown("Restart"))
        {
            // Reset scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // If pushing shift, attempt dash
        if(rwplayer.GetButtonDown("Dash"))
        {
            // Get dash direction

            Vector2 moveDirection = new Vector2(rwplayer.GetAxis("MoveHorizontal"),rwplayer.GetAxis("MoveVertical"));

            moveDirection = Quaternion.AngleAxis(-45, Vector3.forward) * moveDirection;

            // Set to absolute direction
            moveDirection.Normalize();
            
            // Player dash in direction
            Player.Instance.Dash(moveDirection);
        
        }

        if (rwplayer.GetButtonDown("Fire1"))
        {
            Player.Instance.BasicAttack();
        }
        
        if (rwplayer.GetButtonDown("Slam"))
        {
            Player.Instance.SuperAttack();
        }

        //change facing direction

        Vector2 rawFacingDirection = new Vector2(rwplayer.GetAxis("LookHorizontal"), rwplayer.GetAxis("LookVertical"));

        rawFacingDirection = Quaternion.AngleAxis(-45, Vector3.forward) * rawFacingDirection;

        Vector3 facingDirection;

        if (rawFacingDirection != Vector2.zero) // if right stick moved
        {
            facingDirection = new Vector3(rawFacingDirection.x, 0, rawFacingDirection.y).normalized;
            Player.Instance.ChangeFacingDirection(facingDirection);
            trackMousePosition = false;
        }
        else if (mouseMoved || trackMousePosition)
        {
            trackMousePosition = true;
            facingDirection = GetPlayerFacingDirection();

            if (facingDirection != Vector3.zero)
            {
                Player.Instance.ChangeFacingDirection(facingDirection);
            }
        }
    }

    /// <summary>
    /// Runs the frame input intended while in the menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunMenuFrameInput()
    {

    }

    /// <summary>
    /// Runs the frame input intended while in the pause InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunPauseFrameInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunGameplayFixedInput()
    {
        
        Vector2 moveDirection = new Vector2(rwplayer.GetAxis("MoveHorizontal"),rwplayer.GetAxis("MoveVertical"));

        moveDirection = Quaternion.AngleAxis(-45, Vector3.forward) * moveDirection;

        // Set to absolute direction
        moveDirection.Normalize();
        
        Player.Instance.Move(moveDirection);

    }

    /// <summary>
    /// Runs the fixed input intended while in the menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunMenuFixedInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunPauseFixedInput()
    {

    }

    // FixedUpdate called once per physics tick
    private void FixedUpdate()
    {
        // based on current InputState handle input appropriately
        switch (currentState)
        {
            case (InputState.MENU):
                RunMenuFixedInput();
                break;
            case (InputState.GAMEPLAY):
                if (!Player.inactive)
                    RunGameplayFixedInput();
                break;
            case (InputState.PAUSE):
                RunPauseFixedInput();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case (InputState.MENU):
                RunMenuFrameInput();
                break;
            case (InputState.GAMEPLAY):
                if (!Player.inactive)
                    RunGameplayFrameInput();
                break;
            case (InputState.PAUSE):
                RunPauseFrameInput();
                break;
            default:
                break;
        }

    }

    private void LateUpdate()
    {
        prevMousePos = Input.mousePosition;
    }
}

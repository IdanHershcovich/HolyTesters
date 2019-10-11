using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement of the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary> standard movement speed of player </summary>
    [SerializeField] private float runSpeed;
    /// <summary> dash speed of player </summary>
    [SerializeField] private float dashSpeed;
    /// <summary> Length of time the dash is active </summary>
    [SerializeField] private float dashTime;
    /// <summary> Cooldown after dashing </summary>
    [SerializeField] private float dashCoolDownTime;
    /// <summary> recovery Deceleration for knockback </summary>
    [SerializeField] private float kbDecel;

    public bool stunned;

    [SerializeField] private ParticleSystem dashEffect;

    /// <summary> Is the player dashing </summary>
    private bool isDashing;
    /// <summary> is the dash cooling down </summary>
    private bool dashCoolingDown;
    /// <summary> is the player running </summary>
    private bool isRunning;

    private bool _canMove;
    /// <summary> Can the player move? </summary>
    public bool canMove { get {return _canMove; } set { _canMove = value ; } }

    /// <summary> reference to the player's object mover </summary>
    private ObjectMover mover;
    /// <summary> reverence to the player's animator </summary>
    private Animator animator;
    /// <summary> Reference to the player's audio controller </summary>
    private PlayerAudioController audioController;

    //TODO remove when isometric is implemented
    private CameraScript cameraScript;

    public float moveSpeedMultiplier = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        mover = GetComponent<ObjectMover>();
        animator = GetComponentInChildren<Animator>();
        cameraScript = Camera.main.GetComponent<CameraScript>();
        audioController = GetComponent<PlayerAudioController>();

        canMove = true;
    }

    /// <summary>
    /// Move player by current speed and given direction
    /// </summary>
    /// <param name="rawDirection"> The direction the player runs in</param>
    public void MovePlayer(Vector2 rawDirection)
    {

        Vector3 direction = new Vector3(rawDirection.x, 0, rawDirection.y);
        animator.SetFloat("X", rawDirection.x);
        animator.SetFloat("Y", rawDirection.y);
        //ensure the direction is a normalized vector
        direction.Normalize();

        //If no longer running
        if (direction == Vector3.zero || !canMove || isDashing || direction == Vector3.zero)
        {
            animator.SetFloat("Speed", 0.0f);
            return;
        }

        // if the player just started running
        if (!isRunning)
        {
            animator.SetFloat("Speed", runSpeed * moveSpeedMultiplier);
        }
      
        //move with run speed
        mover.Move(direction * runSpeed * moveSpeedMultiplier * Time.fixedDeltaTime);
        //set facing direction to the moving direction
        //transform.forward = new Vector3(direction.x, 0, direction.y);
    }

    //TODO remove when isometric is implemented
    private void OnTriggerEnter(Collider other)
    {

        // if triggered room transitioner
        if (other.tag == "RoomTransitioner")
        {
            // set new camera center
            //cameraScript.ChangeCenter(other.transform.parent.transform.position);

            EscapePrevention ep;

            //if this room's escape prevention system hasn't been triggered yet, start it
            if ((ep = other.transform.parent.transform.parent.GetComponentInChildren<EscapePrevention>()) != null)
                other.transform.parent.transform.parent.GetComponentInChildren<EscapePrevention>().Begin();
        }
    }

    /// <summary>
    /// Initiates the player dash
    /// </summary>
    /// <param name="direction"> Direction of the dash</param>
    public void InitiateDash(Vector2 rawDirection) 
    {

        Vector3 direction = new Vector3(rawDirection.x, 0, rawDirection.y);
        // if unable to dash, return
        if (!canMove || isDashing || dashCoolingDown || direction == Vector3.zero)
            return;
        else
            StartCoroutine("Dash", direction);
    }

    /// <summary>
    /// The coroutine that handles dashing
    /// </summary>
    /// <param name="direction"> direction of the dash </param>
    private IEnumerator Dash(Vector3 direction) 
    {
        dashEffect.Play();
        SoundController.Instance.PlaySoundEffect(SoundType.DASH);

        // initiate animation 
        animator.SetTrigger("Dash");

        //Set facing direction to direction off dash
        //transform.forward = new Vector3(direction.x, 0, direction.y);

        isDashing = true;
        
        // for the length of dash time, move the player with the dash speed
        for (float timer = 0; timer < dashTime; timer += Time.fixedDeltaTime)
        {
            if (!canMove)
                break;
            mover.Move(direction * dashSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        //End dashing

        //Use commented out line to destroy particles on stop
        //dashEffect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        dashEffect.Stop();

        isDashing = false;
        dashCoolingDown = true;

        // cooldown dash
        for (float timer = 0; timer < dashCoolDownTime; timer += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        dashCoolingDown = false;
        
    }
    
    public void TakeKnockBack(Vector3 direction, float speed, float kbStunTimer) 
    {
        StartCoroutine(Knockback(direction * speed, kbStunTimer));
    }
    
    private IEnumerator Knockback(Vector3 velocity, float kbStunTimer)
    {
        canMove = false;
        animator.SetTrigger("Stunned");
        for (float currSpeed = velocity.magnitude; currSpeed > 0; currSpeed -= kbDecel * Time.fixedDeltaTime)
        {
            mover.Move(velocity.normalized * currSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        for (float timer = 0; timer < kbStunTimer; timer += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        canMove = true;

    }

    public void ChangeFacingDirection(Vector3 newDirection)
    {
        transform.forward = newDirection;
    }

}

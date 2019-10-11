using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to move object with or without collision detection
/// Used to prevent rigid body from applying physics when all that's wanted is preventing of clipping
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class ObjectMover : MonoBehaviour
{

    /// <summary> Reference to the objects rigidbody </summary>
    Rigidbody rb;
    /// <summary> Does the object have a rigidbody </summary>
    private bool hasRigidbody;

    private bool isContinuous;

    [HideInInspector]
    public int layerMask;

    [HideInInspector]
    public CharacterController charCont;

    public delegate void HitEvent(ControllerColliderHit hit);

    public event HitEvent ObjectMoverHit;

    // Start is called before the first frame update
    void Start()
    {

        SetLayerMask();

        charCont = GetComponent<CharacterController>();

        // issue with player that set's it's position below surface, this negates it, but should look into actual fix soon
        if (tag == "Player")
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        // if rigid body set values to be appropriate for this game
        if ((rb = GetComponent<Rigidbody>()) != null && !rb.isKinematic)
        {
            hasRigidbody = true;
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }
    }

    private void SetLayerMask()
    {
        int layer = gameObject.layer;
        layerMask = 0;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(layer, i))
                layerMask |= 1 << i;
        }

    }

    /// <summary>
    /// Moves an object a certain distance
    /// Should be used with fixed update
    /// Does not require rigidbody
    /// Can be used with rigidbody for collision detection
    /// </summary>
    /// <param name="moveBy"> The ammount to move the object by</param>
    public void Move(Vector3 moveBy)
    {

        float prevY = transform.position.y;
        
        // for continous collision detection
        charCont.Move(moveBy);

        //Makes sure the character controller didn't step up above any collider
        if (transform.position.y != prevY)
        {
            transform.position = new Vector3(transform.position.x, prevY, transform.position.z);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasRigidbody && !rb.isKinematic)
            rb.velocity = Vector3.zero;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(ObjectMoverHit != null)
            ObjectMoverHit(hit);
    }
}

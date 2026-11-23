using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInteractor))]
public class PlayerThrow : MonoBehaviour
{
    [Tooltip("The transform of the player camera")]
    public Transform camPos;

    [Tooltip("What layers can our attached throwable object collide with")]
    public LayerMask collisionDetection;

    [SerializeField]
    [Tooltip("The offset distance our throwable object will be held from the player")]
    float offset;

    [SerializeField]
    [Tooltip("How fast will our throwable object follow our camera (adds a little drag)")]
    float lookSpeed;

    // Eases rotation towards the cameras rotation
    Quaternion smoothedRot;
    // Eases distance towards the fixed distance
    float smoothedDist;

    // Used for rudamentary collider fixes when an object is detached
    bool insideOfSomething = false;

    Throwable attachedThrow;
    private PlayerInteractor playerInteractor;
    bool interactPressed, throwPressed;

    private void Start()
    {
        playerInteractor = this.GetComponent<PlayerInteractor>();
    }

    void Update()
    {
        if (attachedThrow != null)
            HandleInput();
    }

    private void FixedUpdate()
    {
        if(attachedThrow != null)
        {
            if (interactPressed)
            {
                attachedThrow.Drop();
                DetachObject();
            }
            else if (throwPressed)
            {
                Vector3 throwDir = camPos.transform.TransformDirection(Vector3.forward);
                attachedThrow.Throw(throwDir);
                DetachObject();
            }
        }
    }

    private void LateUpdate()
    {
        if (attachedThrow != null)
            HandleHoldMovement();
    }

    /// <summary>
    /// Handles input for throwing
    /// </summary>
    void HandleInput()
    {
        if (Input.GetButtonDown("Interact"))
            interactPressed = true;
        if (Input.GetButtonDown("Throw"))
            throwPressed = true;
    }

    /// <summary>
    /// Handles the movement of the object as you move your camera
    /// </summary>
    void HandleHoldMovement()
    {
        float fixDistance = offset;
        RaycastHit hit;
        //Rudamentary, but serves the purpose of emulating collisions well enough
        if(Physics.Raycast(camPos.transform.position, camPos.transform.TransformDirection(Vector3.forward), out hit, offset, collisionDetection))
        {
            fixDistance = hit.distance;
            insideOfSomething = true;
        }
        else
        {
            insideOfSomething = false;
        }

        // Eases rotation towards the cameras rotation
        smoothedRot = Quaternion.Slerp(smoothedRot, camPos.transform.rotation, lookSpeed * Time.deltaTime);
        // Eases distance between the camera and the held object towards the fixed distance
        smoothedDist = Mathf.Min(fixDistance, Mathf.Lerp(smoothedDist, fixDistance, lookSpeed * Time.deltaTime));

        // The held position is somewhere along where the camera actually sits, modified by the smoothed values
        Vector3 holdPos = camPos.transform.position + smoothedRot * Vector3.forward * smoothedDist;

        attachedThrow.transform.position = holdPos;
    }

    /// <summary>
    /// Attaches a throwable object to the player
    /// </summary>
    public void AttachObject(Throwable attach)
    {
        attachedThrow = attach;
        playerInteractor.enabled = false;
        Physics.IgnoreLayerCollision(3, 7, true);
        
        smoothedRot = camPos.transform.rotation;
        smoothedDist = Vector3.Distance(camPos.transform.position, attachedThrow.transform.position);
    }

    /// <summary>
    /// Detaches a throwable object from the player
    /// </summary>
    public void DetachObject()
    {
        if(insideOfSomething)
        {
            attachedThrow.rb.AddForce(transform.position, ForceMode.Impulse);
            insideOfSomething = false;
        }
        attachedThrow = null;
        playerInteractor.enabled = true;
        Physics.IgnoreLayerCollision(3, 7, false);

        interactPressed = false;
        throwPressed = false;
    }
}

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
            HandleHoldMovement();
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
        }

        Debug.DrawRay(camPos.transform.position, camPos.transform.TransformDirection(Vector3.forward) * fixDistance, Color.white);
        Vector3 holdPos = camPos.transform.position + camPos.transform.TransformDirection(Vector3.forward) * fixDistance;
        attachedThrow.rb.MovePosition(Vector3.Lerp(attachedThrow.gameObject.transform.position, holdPos, Time.fixedDeltaTime * lookSpeed));
    }

    /// <summary>
    /// Attaches a throwable object to the player
    /// </summary>
    public void AttachObject(Throwable attach)
    {
        attachedThrow = attach;
        playerInteractor.enabled = false;
        Physics.IgnoreLayerCollision(3, 7, true);
    }

    /// <summary>
    /// Detaches a throwable object from the player
    /// </summary>
    public void DetachObject()
    {
        attachedThrow = null;
        playerInteractor.enabled = true;
        Physics.IgnoreLayerCollision(3, 7, false);

        interactPressed = false;
        throwPressed = false;
    }
}

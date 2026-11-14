using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Tooltip("The transform of the player camera")]
    public Transform camPos;

    [Tooltip("How far away you can interact with objects")]
    public float interactDistance;

    //public TextMeshProUGUI uiText;
    bool interactPressed = false, interactAllowed = true;

    void Update()
    {
        interactPressed = Input.GetKeyDown(KeyCode.E);

        Vector3 rayLocation = camPos.transform.position;
        Debug.DrawLine(camPos.transform.position, camPos.transform.position + camPos.transform.TransformDirection(Vector3.forward) * interactDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, camPos.transform.TransformDirection(Vector3.forward), out hit, interactDistance))
        {
            I_Interactable interactable = hit.collider.gameObject.GetComponent<I_Interactable>();
            if (interactable != null)
                InteractionWithObject(interactable);
        }
    }

    /// <summary>
    /// Called when a valid object is interacted with.
    /// Broadcasts the description text of the object and calls the objects interacted event.
    /// </summary>
    /// <param name="interactable">interactable Object with I_Interactable interface attached</param>
    void InteractionWithObject(I_Interactable interactable)
    {
        if (interactPressed && interactAllowed)
        {
            interactable.Interacted(this.gameObject);
            interactAllowed = false;
            Invoke(nameof(ResetInteract), 0.2f);
        }
    }

    /// <summary>
    /// Called to reset the interactAllowed boolean.
    /// Helps add a buffer to interactions.
    /// </summary>
    void ResetInteract()
    {
        interactAllowed = true;
    }
}
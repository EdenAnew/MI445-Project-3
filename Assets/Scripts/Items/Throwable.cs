using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Throwable : MonoBehaviour, I_Interactable
{
    [SerializeField]
    [Tooltip("The description text/name for this throwable.")]
    private string localDescriptionText;
    public string DescriptionText { get { return localDescriptionText; } }

    [HideInInspector]
    public Rigidbody rb;

    [SerializeField]
    [Tooltip("The amount of forward force you'd like for this to have when thrown")]
    float throwMultiplier;
    [SerializeField]
    [Tooltip("The amount of upward force you'd like for this to have when thrown")]
    float upForceMultiplier;

    private bool held;
    protected virtual bool CurrentlyHeld
    {
        get { return held; }
        set { held = value; }
    }

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interacted(GameObject caller)
    {
        PlayerThrow throwScript = caller.GetComponent<PlayerThrow>();
        if (throwScript != null)
        {
            rb.isKinematic = true;
            throwScript.AttachObject(this);
            CurrentlyHeld = true;
        }
    }

    public void BroadcastDescriptionText()
    {
        Debug.Log(localDescriptionText);
    }

    /// <summary>
    /// Movement properties when this object is dropped
    /// </summary>
    public void Drop()
    {
        rb.isKinematic = false;
        CurrentlyHeld = false;
    }

    /// <summary>
    /// Movement properties when this object is thrown
    /// </summary>
    public void Throw(Vector3 throwDirection)
    {
        rb.isKinematic = false;
        rb.AddForce(throwDirection * throwMultiplier + Vector3.up * upForceMultiplier, ForceMode.Impulse);
        CurrentlyHeld = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Interactable
{
    // Description text for this object. Can be broadcasted to UI
    string DescriptionText { get; }

    /// <summary>
    /// Function that will be called when this object has been interacted with.
    /// </summary>
    /// <param name="caller">The gameobject that interacted with this object</param>
    void Interacted(GameObject caller = null);

    /// <summary>
    /// Used to send the description text to the UI/UIManager.
    /// </summary>
    void BroadcastDescriptionText();
}

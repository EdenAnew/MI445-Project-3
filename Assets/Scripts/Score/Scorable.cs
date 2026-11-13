using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Info container for ease of writing + modularity
[System.Serializable]
public struct ScoreInfo
{
    [Tooltip("How many points will this give?")]
    public int points;
    [Tooltip("What's the description text that you'd like associated with this event?")]
    public string description;
    [Tooltip("Is this a crime?")]
    public bool crime;
}

// EVERY SCORABLE OBJECT NEEDS THIS ABSTRACT CLASS
// I think this is an ACTUAL good use of an abstract class.
// There are several concrete implementations (mostly useful for OnEnable and OnDisable
// but also would you rather read Score() or OnScored?.Invoke(ScoringInfo) in every script?
//
// There is still a enforcement of subclasses implementing some kind of Score Info (which
// is the main benefit of using an interface over an abstract class, IMO)
//
// Overall, this seemed to keep the concrete implementation way cleaner than how it looked
// with an interface (WRITING LESS CODE IS ALWAYS GOOD! BE LAZY!)
public abstract class Scorable : MonoBehaviour
{
    // Every scorable will need an info container
    public abstract ScoreInfo ScoringInfo { get; }

    // The event used to broadcast that our scorable scored
    public event System.Action<ScoreInfo> OnScored;


    /// <summary>
    /// Broadcast that this object has scored, using info from ScoringInfo 
    /// </summary>
    virtual protected void Score()
    {
        OnScored?.Invoke(ScoringInfo);
    }

    virtual protected void OnEnable()
    {
        // Subscribes the ScoreManager to our event
        ScoreManager.instance?.SubscribeToScorable(this);
    }

    virtual protected void OnDisable()
    {
        // Unsubscribes the ScoreManager to our event
        ScoreManager.instance?.UnsubscribeToScorable(this);
    }
}

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


public class Scorable : MonoBehaviour
{
    [Tooltip("All the info that will be sent out when this is scored")]
    [SerializeField] ScoreInfo scoreInfo;
    public ScoreInfo ScoringInfo { get { return scoreInfo; } }

    // The event used to broadcast that our scorable scored
    [HideInInspector]
    public static event System.Action<ScoreInfo> OnScored;

    /// <summary>
    /// Broadcast that this object has scored, using info from ScoringInfo 
    /// </summary>
    virtual public void Score()
    {
        OnScored?.Invoke(ScoringInfo);
    }
}

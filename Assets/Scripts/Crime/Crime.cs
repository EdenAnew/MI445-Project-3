using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Info container for ease of writing + modularity
[System.Serializable]
public struct CrimeInfo
{
    [Tooltip("Was this a main crime?")]
    public bool main;
}

public class Crime : MonoBehaviour
{
    // The event used to broadcast that our crime was committed
    [HideInInspector]
    public static event System.Action OnCrimeCommitted;

    // This boolean disables the crime from being committed.
    // I chose to have this bool instead of calling Destroy() so that
    // future programmers don't have to worry about writing if getComponent<>() != null
    // every time they access the object. the performance increase should really be minimal
    // and it saves on development time!
    bool disabled = false;


    /// <summary>
    /// Broadcast that this crime has been committed 
    /// </summary>
    virtual public void CommitCrime()
    {
        if (disabled) return;

        OnCrimeCommitted?.Invoke();
        disabled = true;
    }
}

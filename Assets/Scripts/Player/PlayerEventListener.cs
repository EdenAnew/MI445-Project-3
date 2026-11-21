using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        Crime.OnCrimeCommitted += RechargeDash;
    }

    private void OnDisable()
    {
        Crime.OnCrimeCommitted -= RechargeDash;
    }

    void RechargeDash(CrimeInfo info)
    {
        Debug.Log("It's all hooked up! Looks like you committed " + (info.main ? "a main crime." : "a minor crime."));
    }
}

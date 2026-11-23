using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventListener : MonoBehaviour
{
    PlayerMovement pMovement;

    private void Start()
    {
        pMovement = GetComponent<PlayerMovement>();
    }

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
        Debug.Log("Looks like you committed " + (info.main ? "a main crime." : "a minor crime."));
        pMovement.RegainDashCharge();
    }
}

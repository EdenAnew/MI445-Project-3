using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crime))]
[RequireComponent(typeof(ScorableTimed))]
public class TrespassTrigger : MonoBehaviour
{
    private ScorableTimed scoringComponent;
    private Crime crimeComponent;

    private void Start()
    {
        crimeComponent = GetComponent<Crime>();
        scoringComponent = GetComponent<ScorableTimed>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player is in our trigger
        if (other.tag == "Player")
        {
            crimeComponent.CommitCrime();
            scoringComponent.StartScoring();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            scoringComponent.StopScoring();
        }
    }

    
}

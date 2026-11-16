using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScorableTimed))]
public class TrespassTrigger : MonoBehaviour
{
    private ScorableTimed scoringComponent;

    private void Start()
    {
        scoringComponent = GetComponent<ScorableTimed>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player is in our trigger
        if (other.tag == "Player")
        {
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

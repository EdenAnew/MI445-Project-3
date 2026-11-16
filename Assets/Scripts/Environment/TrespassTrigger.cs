using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scorable))]
public class TrespassTrigger : MonoBehaviour
{
    private Scorable scoringComponent;


    [Tooltip("How much time needs to elapse before scoring")]
    [SerializeField] float timeUntilPointsScored = 3;

    float timer = 0;

    private void Start()
    {
        scoringComponent = GetComponent<Scorable>();
    }

    private void OnTriggerStay(Collider other)
    {
        // If player is in our trigger
        if (other.tag == "Player")
        {
            // Increment time until a scoring event
            timer += Time.deltaTime;
            if(timer >= timeUntilPointsScored)
            {
                scoringComponent.Score();
                timer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            timer = 0;
        }
    }

    
}

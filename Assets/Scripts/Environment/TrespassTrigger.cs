using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrespassTrigger : Scorable
{
    [SerializeField] ScoreInfo scoreInfo;
    public override ScoreInfo ScoringInfo { get { return scoreInfo; } }


    [Tooltip("How much time needs to elapse before scoring")]
    [SerializeField] float timeUntilPointsScored = 3;

    float timer = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            timer += Time.deltaTime;
            if(timer >= timeUntilPointsScored)
            {
                Score();
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

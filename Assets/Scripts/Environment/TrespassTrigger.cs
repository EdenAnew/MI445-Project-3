using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrespassTrigger : Scorable
{
    [SerializeField] ScoreInfo scoreInfo;
    public override ScoreInfo ScoringInfo { get { return scoreInfo; } }


    [Tooltip("How much time needs to elapse before scoring")]
    [SerializeField] float timeUntilPointsScored = 3;
    bool trespassing = false;
    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (trespassing && timer >= timeUntilPointsScored)
        {
            Score();
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            trespassing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            trespassing = false;
            timer = 0;
        }
    }

    
}

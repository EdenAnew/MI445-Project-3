using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScorableTimed))]
public class IllegalThrowable : Throwable
{
    private ScorableTimed scoringTimer;

    override protected bool CurrentlyHeld
    {
        get { return base.CurrentlyHeld; }
        set
        {
            base.CurrentlyHeld = value;

            if (value == true)
                scoringTimer.StartScoring();
            else
                scoringTimer.StopScoring();
        }
    }

    override protected void Start()
    {
        base.Start();
        scoringTimer = GetComponent<ScorableTimed>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorableTimed : Scorable
{
    [SerializeField]
    [Tooltip("The amount of time that must pass before score is triggered.")]
    float timeUntilScore = 1f;

    /// <summary>
    /// Starts the scoring coroutine loop
    /// </summary>
    public void StartScoring()
    {
        StopCoroutine("CountdownToScore");
        StartCoroutine("CountdownToScore");
    }

    /// <summary>
    /// Stops the scorign coroutine
    /// </summary>
    public void StopScoring()
    {
        StopCoroutine("CountdownToScore");
    }

    /// <summary>
    /// Waits a certain amount of seconds before scoring, and if still valid,
    /// it scores.
    /// </summary>
    IEnumerator CountdownToScore()
    {   
        yield return new WaitForSeconds(timeUntilScore);

        Score();
        StartScoring();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;
    int totalScore = 0;

    // Basic singleton stuff
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Subscribes to a Scorable's event(s)
    /// </summary>
    /// <param name="scorable">The scorable to subscribe from</param>
    public void SubscribeToScorable(Scorable scorable)
    {
        scorable.OnScored += AddScore;
    }

    /// <summary>
    /// Unsubscribes to a Scorable's event(s)
    /// </summary>
    /// <param name="scorable">The scorable to unsubscribe from</param>
    public void UnsubscribeToScorable(Scorable scorable)
    {
        scorable.OnScored -= AddScore;
    }

    /// <summary>
    /// Adds score to the total.
    /// Broadcasts local score event and total score to UI.
    /// </summary>
    /// <param name="score">The information associated with the recent scoring object</param>
    void AddScore(ScoreInfo score)
    {
        string crimeText = score.crime ? " a crime " : " not a crime, but it was ";
        Debug.Log("SCORED! " + score.description + " was" + crimeText + "worth " + score.points + " points!");

        totalScore += score.points;

        Debug.Log("Current total score: " + totalScore);

        // IM GOING TO MAKE AN EVENT BUS FOR UI WHERE ILL BROADCAST THE TOTAL SCORE + LOCAL SCORE
    }
}

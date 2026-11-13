using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;
    int totalScore = 0;

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

    public void SubscribeToScorable(Scorable scorable)
    {
        scorable.OnScored += AddScore;
    }

    public void UnsubscribeToScorable(Scorable scorable)
    {
        scorable.OnScored -= AddScore;
    }

    void AddScore(ScoreInfo score)
    {
        Debug.Log("I'm adding score");
        string crimeText = score.crime ? " a crime " : " not a crime, but it was ";
        Debug.Log("SCORED! " + score.description + " was" + crimeText + "worth " + score.points + " points!");
        totalScore += score.points;
        // IM GOING TO MAKE AN EVENT BUS FOR UI WHERE ILL BROADCAST THE TOTAL SCORE
        Debug.Log("Current total score: " + totalScore);
    }
}

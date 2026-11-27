using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;
    int totalScore = 0;
    public int TotalScore { get { return totalScore; } }

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

    private void OnEnable()
    {
        Scorable.OnScored += AddScore;
    }

    private void OnDisable()
    {
        Scorable.OnScored -= AddScore;
    }

    /// <summary>
    /// Adds score to the total.
    /// Broadcasts local score event and total score to UI.
    /// </summary>
    /// <param name="score">The information associated with the recent scoring object</param>
    private void AddScore(ScoreInfo score)
    {
        string crimeText = score.crime ? " a crime " : " not a crime, but it was ";
        Debug.Log("SCORED! " + score.description + " was" + crimeText + "worth " + score.points + " points!");

        totalScore += score.points;

        Debug.Log("Current total score: " + totalScore);
    }
}

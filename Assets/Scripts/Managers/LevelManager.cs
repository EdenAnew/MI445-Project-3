using UnityEngine;
using TMPro;


public class LevelManager: MonoBehaviour
{
    public static LevelManager instance = null;

    [Tooltip("The scriptable object that corresponds to this level.")]
    public LevelBaseData levelInfo;

    [Tooltip("Starts the game timer")]
    public bool startGame;

    [Header("Game Time")]
    [Tooltip("Enter the total game time in minutes here here. Enter '5.5' for 5:30 seconds")]
    public float gameTimeMinutes;
    private float gameTimeSeconds;
    public TextMeshProUGUI gameTimeText;


    // Has the main crime been committed?
    private bool mainCrimeCommitted = false;


    // TESTING CODE 1 - PLEASE REMOVE IF NOT COMMENTED
    //[SerializeField] LevelPrerequsiteScoreTest testPrerequsites;

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

    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;

        // Maybe change later, good for testing, though
        startGame = true;
    }

    void Update()
    {
        HandleTimer();
    }

    /// <summary>
    /// Handles converting the level timer to text,
    /// as well as ending the game when timer ends.
    /// 
    /// Not a coroutine for easier text handling(?)
    /// </summary>
    void HandleTimer()
    {
        if (gameTimeText != null)
            TimerToText();
        if (startGame)
        {
            gameTimeSeconds -= Time.deltaTime;
            if (gameTimeSeconds <= 0)
            {
                LevelLost();
            }
        }
    }

    /// <summary>
    /// Called when the level is lost.
    /// </summary>
    void LevelLost()
    {
        // insert future logic when time runs out, player caught
        Debug.Log("You just lost.");
        startGame = false;
    }

    /// <summary>
    /// Called when the level is won.
    /// </summary>
    void LevelWon()
    {
        // insert future logic when time runs out, player caught
        Debug.Log("You just won.");
        startGame = false;

        // Probably temp code. I'm tired.
        int totalScore = 0;
        if (ScoreManager.instance != null)
            totalScore = ScoreManager.instance.TotalScore;

        LevelResults results = new LevelResults(totalScore, (gameTimeMinutes * 60) - gameTimeSeconds);
        SaveManager.instance?.SaveLevelResults(levelInfo, results);

        // TESTING CODE 1 - PLEASE REMOVE IF NOT COMMENTED
        //testPrerequsites.AttemptUpdateText();
    }

    /// <summary>
    /// When a crime is committed, we check if it was the main crime,
    /// and set our main crime flag to true if it was.
    /// </summary>
    void CheckForMainCrime(CrimeInfo crime)
    {
        if(crime.main)
        {
            mainCrimeCommitted = true;
            Debug.Log("GameManager: The main crime was committed!");
        }
        else
        {
            Debug.Log($"You just committed a minor crime: {crime.type}");
        }
    }

    /// <summary>
    /// The logic the GameManager undergoes when attempting to go through
    /// a level exit.
    /// </summary>
    public void AttemptExit()
    {
        if(mainCrimeCommitted)
        {
            LevelWon();
        }
        else
        {
            Debug.Log("You tried to leave, but you haven't committed the main crime yet.");
        }
    }

    /// <summary>
    /// Converts the level timer to text
    /// </summary>
    public void TimerToText()
    {
        //translates in game time to text mesh pro to be displayed
        int minutes = Mathf.FloorToInt(gameTimeSeconds / 60f);
        float seconds = gameTimeSeconds % 60f;
        gameTimeText.text = $"{minutes:00}:{seconds:00.00}";
    }

    private void OnEnable()
    {
        Crime.OnCrimeCommitted += CheckForMainCrime;
    }

    private void OnDisable()
    {
        Crime.OnCrimeCommitted -= CheckForMainCrime;
    }
}

/// <summary>
/// The results of the level.
/// Mostly for later developmental ease.
/// </summary>
public class LevelResults
{
    public int Score { get; }
    public float TimeInSeconds { get; }

    public LevelResults(int score, float timeInSeconds)
    {
        Score = score;
        TimeInSeconds = timeInSeconds;
    }
}
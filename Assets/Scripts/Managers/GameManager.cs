using UnityEngine;
using TMPro;


public class GameManager: MonoBehaviour
{
    public static GameManager instance = null;

    [Tooltip("Starts the game timer")]
    public bool startGame;

    [Header("Game Time")]
    [Tooltip("Enter the total game time in minutes here here. Enter '5.5' for 5:30 seconds")]
    public float gameTimeMinutes;
    private float gameTimeSeconds;
    public TextMeshProUGUI gameTimeText;


    // Has the main crime been committed?
    private bool mainCrimeCommitted = false;

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

    void HandleTimer()
    {
        if (gameTimeText != null)
            TimerToText();
        if (startGame)
        {
            gameTimeSeconds -= Time.deltaTime;
            if (gameTimeSeconds <= 0)
            {
                startGame = false;
                TimersUp();

            }
        }
    }

    void TimersUp()
    {
        // insert future logic when time runs out
        Debug.Log("You just lost.");
    }


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

    public void AttemptExit()
    {
        if(mainCrimeCommitted)
        {
            Debug.Log("Congrats, you win!");
        }
        else
        {
            Debug.Log("You tried to leave, but you haven't committed the main crime yet.");
        }
    }

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
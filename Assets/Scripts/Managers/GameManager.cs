using UnityEngine;
using TMPro;


public class GameManager: MonoBehaviour
{
    [Header("Game Time")]
    [SerializeField] [Tooltip("Enter the total game time in minutes here here. Enter '5.5' for 5:30 seconds")] public float gameTimeMinutes;
    private float gameTimeSeconds;

    [SerializeField]
    [Tooltip("Once this many seconds progress into the game, enemies will become permanently aware of the player's location.")]
    private float enemyAwareOfPlayerTime = 40;
    bool enemiesAware = false;

    // The event used to broadcast that we hit the enemy time
    public static event System.Action OnEnemyAwarenessTimer;

    [Header("Game Time Text")]
    public TextMeshProUGUI gameTimeText;
    [Tooltip("Starts the game timer")] public bool startGame;
    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;
     
    }

    void Update()
    {
        if (gameTimeText != null)
            TimerToText();
        if (startGame)
        {
            gameTimeSeconds -= Time.deltaTime;
            if (gameTimeSeconds <= 0)
            {
                startGame = false;
                EndGame();
            
            }
            if(!enemiesAware && gameTimeMinutes * 60 - gameTimeSeconds >= enemyAwareOfPlayerTime)
            {
                Debug.Log("Enemies know where you are!");
                OnEnemyAwarenessTimer?.Invoke();
                enemiesAware = true;
            }
        }
 
    }

    public void TimerToText()
    {
        //translates in game time to text mesh pro to be displayed
        int minutes = Mathf.FloorToInt(gameTimeSeconds / 60f);
        float seconds = gameTimeSeconds % 60f;
        gameTimeText.text = $"{minutes:00}:{seconds:00.00}";
    }

    public void EndGame()
    {
        // insert future logic when game is ended
    }
}
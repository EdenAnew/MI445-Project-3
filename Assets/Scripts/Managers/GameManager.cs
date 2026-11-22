using UnityEngine;
using TMPro;


public class GameManager: MonoBehaviour
{
    [Header("Game Time")]
    [SerializeField] [Tooltip("Enter the total game time in minutes here here. Enter '5.5' for 5:30 seconds")] public float gameTimeMinutes;
    private float gameTimeSeconds;
    [Header("Game Time Text")]
    public TextMeshProUGUI gameTimeText;
    [Tooltip("Starts the game timer")] public bool startGame;
    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;
     
    }

    void Update()
    {
        TimerToText();
        if (startGame)
        {
            gameTimeSeconds -= Time.deltaTime;
            if (gameTimeSeconds <= 0)
            {
                startGame = false;
                EndGame();
            
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
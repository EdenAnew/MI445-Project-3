using UnityEngine;
using TMPro;


public class GameTimer : MonoBehaviour
{
    [SerializeField] [Tooltip("Enter the total game time in minutes here here. Enter '5.5' for 5:30 seconds")] public float gameTimeMinutes;
    private float gameTimeSeconds;
    public TextMeshPro gameTimeText;
    
    
    
    void Start()
    {
        gameTimeSeconds = gameTimeMinutes * 60;
     
    }

    void Update()
    {
        gameTimeSeconds -= Time.deltaTime;
        if (gameTimeSeconds <= 0)
        {
            
        }
    }
}
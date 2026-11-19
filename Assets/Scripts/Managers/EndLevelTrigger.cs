using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("There is no GameManager component on the scene!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if the player touches the collider, it ends the game on the game managers side
        if (other.tag == "Player")
        {
            gameManager.startGame = false;
            gameManager.EndGame();
        }
    }
}

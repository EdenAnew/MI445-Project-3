using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Spawners
{
    [Tooltip("The location you'd like to spawn an enemy.")]
    public Transform spawnTransform;
    [Tooltip("The time at which you'd like an enemy to spawn, at the location specified.")]
    public float spawnTime;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab for the enemies.")]
    private GameObject enemy;

    [Header("Enemy Player Awareness")]

    [SerializeField]
    [Tooltip("Once this many seconds progress into the game, enemies will become permanently aware of the player's location permanently.")]
    private float enemyAwareOfPlayerTime = 40;

    // The event used to broadcast that we hit the enemy time
    public static event System.Action OnEnemyAwarenessTimer;

    // The event call is for enemies that have already spawned, this variable is nice for enemies that havent spawned yet.
    // The event is also nice if we want a UI event.
    bool enemiesKnow = false;

    [Header("Enemy Spawning")]

    [SerializeField]
    [Tooltip("The list of spawners for enemies. Spawners include a location, and a time for when the spawning occurs.")]
    private Spawners[] spawners;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnemyAwarenessTimer");
        for(int i = 0; i < spawners.Length; i++)
        {
            StartCoroutine("SpawnEnemy", i);
        }

    }

    IEnumerator EnemyAwarenessTimer()
    {
        yield return new WaitForSeconds(enemyAwareOfPlayerTime);
        Debug.Log("Enemies know where you are!");
        OnEnemyAwarenessTimer?.Invoke();
        enemiesKnow = true;
    }

    IEnumerator SpawnEnemy(int index)
    {
        yield return new WaitForSeconds(spawners[index].spawnTime);
        GameObject enemyInstance = Instantiate(enemy, spawners[index].spawnTransform.position, Quaternion.identity);
        Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
        
        if (enemyScript == null)
        {
            Debug.LogWarning("The thing you're trying to spawn in EnemyManager doesn't have an enemy script?");
        }
        else if (enemiesKnow)
        {
            enemyScript.GainPlayerAwareness();
        }
    }
}

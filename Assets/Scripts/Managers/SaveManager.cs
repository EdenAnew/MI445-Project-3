using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;
    // The actual save data structure
    private GameSave gameSave;
    // The file path we're saving the save data
    private string savePath;

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

        DontDestroyOnLoad(gameObject);

        transform.parent = null;
        savePath = Path.Combine(Application.persistentDataPath, "save.json");

        LoadGame();
    }

    /// <summary>
    /// Loads the game from the json file, if it exists
    /// </summary>
    void LoadGame()
    {
        if(File.Exists(savePath))
            gameSave = JsonUtility.FromJson<GameSave>(File.ReadAllText(savePath)); 
        else
            gameSave = new GameSave();

        Debug.Log("Game Data Loaded!");
    }

    /// <summary>
    /// Saves the game to the json file.
    /// </summary>
    void SaveGame()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(gameSave, true));

        Debug.Log("Game Data Saved!");
    }

    /// <summary>
    /// Creates new level save data for a level.
    /// </summary>
    /// <param name="newLevelBaseData">The base data for the new level save we'll create</param>
    /// <returns></returns>
    LevelSaveData CreateNewLevelSaveData(LevelBaseData newLevelBaseData)
    {
        LevelSaveData newLevel = new LevelSaveData(newLevelBaseData.LevelID);
        gameSave.levelData.Add(newLevel);
        SaveGame();
        return newLevel;
    }

    /// <summary>
    /// Get the level save data of some level.
    /// </summary>
    /// <param name="requestedLevel">The level that you'd like the save data for.</param>
    /// <returns></returns>
    public LevelSaveData GetLevelSaveData(LevelBaseData requestedLevel)
    { 
        foreach (LevelSaveData level in gameSave.levelData)
        {
            if (level.levelID == requestedLevel.LevelID)
                return level;
        }
        
        // If we get past the for loop without returning the level, it follows that
        // there must not exist any save data for the level with the passed id.
        // Therefore, we create it and return.
        return CreateNewLevelSaveData(requestedLevel);
    }

    /// <summary>
    /// Saves the completed levels results by attempting to update any
    /// level save data.
    /// </summary>
    /// <param name="completedLevel">The level that was just completed</param>
    /// <param name="results">The results of the level that was just completed</param>
    public void SaveLevelResults(LevelBaseData completedLevel, LevelResults results)
    {
        LevelSaveData requestedSave = GetLevelSaveData(completedLevel);
        requestedSave.ApplyNewData(completedLevel, results);
        SaveGame();
    }
}

/// <summary>
/// The bundle of information that our game is saving.
/// Right now, this is just level save data.
/// </summary>
[System.Serializable]
class GameSave
{
    public List<LevelSaveData> levelData = new List<LevelSaveData>();
}

/// <summary>
/// A class outlining what each level should be saving after it is finished.
/// </summary>
[System.Serializable]
public class LevelSaveData
{
    public LevelSaveData(string id)
    {
        levelID = id;
    }

    // The ID of this level
    public string levelID;

    // The best time the player has gotten in this level, in seconds
    public float bestTimeInSeconds = 0;

    // The best score the player has gotten in this level
    public int bestScore = 0;

    // Whether or not this level has been completed before
    public bool completed = false;

    /// <summary>
    /// Compares session level completed results to all-time best results.
    /// Saves whichever is best.
    /// </summary>
    /// <param name="results">This sessions level completion results.</param>
    public void ApplyNewData(LevelBaseData level, LevelResults results)
    {
        if (results.Score > bestScore)
            bestScore = results.Score;

        if (results.TimeInSeconds < bestTimeInSeconds)
            bestTimeInSeconds = results.TimeInSeconds;

        if(level.MinimumScore > bestScore)
            completed = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This creates scriptable objects outlining the basic data of levels that come bundled with the game.
/// Note that this is not the same as LevelSaveData.
/// </summary>
[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelBaseData : ScriptableObject
{
    [Tooltip("The actual ID of this level that we should use for identification (eg. \"Level1\")")]
    [SerializeField] string levelID;

    [Tooltip("The name of this level (eg. \"Monument Madness\" or \"Nuketown\"")]
    [SerializeField] string levelName;

    [Tooltip("The minimum score required to pass this level and move onto the next")]
    [SerializeField] int minimumScore;

    [Tooltip("The levels that need to be beaten with minimum score for this one to unlock.")]
    [SerializeField] LevelBaseData[] prerequisiteLevels;


    /// <summary>
    /// The ID of the level. This is probably what you want for most code communication.
    /// </summary>
    public string LevelID { get { return levelID; } }
    /// <summary>
    /// The name of the level. This is some string that the user may see (eg. "Repossession")
    /// </summary>
    public string LevelName { get { return levelName; } }
    /// <summary>
    /// The minimum score required to pass this level and move onto the next.
    /// </summary>
    public int MinimumScore { get { return minimumScore; } }
    /// <summary>
    /// The levels that need to be beaten with minimum score for this one to unlock.
    /// </summary>
    public LevelBaseData[] Prerequsites { get { return prerequisiteLevels; } }
}

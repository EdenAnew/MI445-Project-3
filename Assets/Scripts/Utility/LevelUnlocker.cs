using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker
{
    /// <summary>
    /// Checks if the level is unlocked.
    /// </summary>
    /// <param name="save">The SaveManager instance in the scene</param>
    /// <param name="level">The level that we're examining</param>
    /// <returns></returns>
    public static bool IsLevelUnlocked(SaveManager save, LevelBaseData level)
    {
        
        foreach (LevelBaseData prerequsite in level.Prerequsites)
        {
            LevelSaveData savedData = save.GetLevelSaveData(prerequsite);

            if (savedData.bestScore < prerequsite.MinimumScore)
                return false;
        }

        return true;
    }
}

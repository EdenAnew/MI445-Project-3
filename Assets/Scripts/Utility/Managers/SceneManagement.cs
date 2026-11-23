using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Reloads the current scene
    /// </summary>
    public void ReloadScene()
    {
        Debug.Log("SceneManagement.cs: Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Proceeds to the next scene in the build index
    /// </summary>
    public void ProceedToNextSceneInBuild()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings >= nextScene)
        {
            Debug.Log("SceneManagement.cs: ProceedToNextSceneInBuild() called when max index is already achieved. Looping back to first scene." +
                "\nNOTE: This can also occur if your scene has not been added to build settings! Please double check!");
            nextScene = 0;
        }
        else if (SceneManager.sceneCountInBuildSettings == 0)
        {
            Debug.LogError("ERROR in SceneManagement.cs: ProceedToNextSceneInBuild() called with no scenes added to build settings!");
            return;
        }

        SceneManager.LoadScene(nextScene);
    }

    /// <summary>
    /// Loads a specific scene by name
    /// </summary>
    /// <param name="name">The name of the scene you'd like to load</param>
    public void LoadSceneByName(string name)
    {
        if(SceneManager.GetSceneByName(name) == null)
        {
            Debug.LogError("ERROR in SceneManagement.cs: LoadSceneByName() could not find a scene named " + name + ". Check" +
                " for a possible typo and ensure that it is in the build settings.");
            return;
        }
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Load a specific scene by index number
    /// </summary>
    /// <param name="i">The desired scene index</param>
    public void LoadSceneByBuildIndex(int i)
    {
        if (SceneManager.GetSceneByBuildIndex(i) == null)
        {
            Debug.LogError("ERROR in SceneManagement.cs: LoadSceneByBuildIndex() could not find a scene at index " + i + ". Please" +
                " check the build settings.");
            return;
        }
        SceneManager.LoadScene(i);
    }
}

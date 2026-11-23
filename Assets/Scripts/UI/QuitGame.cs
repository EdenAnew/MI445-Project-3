using UnityEngine;
using UnityEngine.UI;

//quits game in editor/build
public class QuitGame : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Quit);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

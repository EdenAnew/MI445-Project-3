using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelPrerequsiteScoreTest : MonoBehaviour
{
    [SerializeField] LevelBaseData level;
    TextMeshProUGUI textbox;
    private void Start()
    {
        textbox = this.GetComponent<TextMeshProUGUI>();
    }

    public void AttemptUpdateText()
    {
        if(LevelUnlocker.IsLevelUnlocked(SaveManager.instance, level))
            textbox.text = "Next Level Is Unlocked";
    }
}

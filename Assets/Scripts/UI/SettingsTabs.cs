using UnityEngine;
using UnityEngine.UI;

//script  for switching between settings tabs
//yes i know its very boof but it works okay
public class SettingsTabs : MonoBehaviour
{
    [SerializeField] Transform tabs;
    [SerializeField] Transform pages;

    void Start()
    {
        foreach (Transform tab in tabs)
        {
            string pageName = tab.name;
            tab.GetComponent<Button>().onClick.AddListener(() => Show(pages, pageName));
        }

        Show(pages, tabs.GetChild(0).name);
    }

    static void Show(Transform pages, string pageName)
    {
        foreach (Transform page in pages)
            page.gameObject.SetActive(page.name == pageName);
    }
}

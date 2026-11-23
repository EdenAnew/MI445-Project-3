using UnityEngine;
using UnityEngine.UI;

//shows the tick while this button is toggled on
public class ToggleTick : MonoBehaviour
{
    [SerializeField] GameObject tick;
    [SerializeField] bool isOn;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Toggle);
        tick.SetActive(isOn);
    }

    void Toggle()
    {
        isOn = !isOn;
        tick.SetActive(isOn);
    }
}

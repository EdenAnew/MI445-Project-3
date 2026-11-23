using UnityEngine;
using UnityEngine.EventSystems;
//simple script from other projects that shows something when hovered over
public class LevelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hover;
    [SerializeField] GameObject info;

    void Awake() => SetVisible(false);

    public void OnPointerEnter(PointerEventData eventData) => SetVisible(true);

    public void OnPointerExit(PointerEventData eventData) => SetVisible(false);

    void SetVisible(bool visible)
    {
        hover.SetActive(visible);
        info.SetActive(visible);
    }
}

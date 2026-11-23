
//controls the main menu UI, mostly the cameras because of the diegetic menus
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] stations;
    [SerializeField] GraphicRaycaster[] raycasters;
    [SerializeField] CinemachineBrain brain;

    int current;

    void Start() => Focus(0);

    public void GoTo(int index)
    {
        if (brain.IsBlending || index == current) return;
        Focus(index);
    }

    void Focus(int index)
    {
        for (int i = 0; i < stations.Length; i++)
        {
            stations[i].Priority = i == index ? 20 : 0;
            if (i < raycasters.Length)
                raycasters[i].enabled = i == index;
        }
        current = index;
    }
}


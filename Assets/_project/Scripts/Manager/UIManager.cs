using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _panelGameOver;
    [SerializeField] private GameObject _panelMainMenu;

    public void ShowPanelGameOver()
    {
        _panelGameOver.SetActive(true);
    }

    public void HidePanelGameOver()
    {
        _panelGameOver.SetActive(false);
    }
}

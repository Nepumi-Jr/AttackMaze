using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNSerttingNMore : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject SettingPanel;
    // Start is called before the first frame update
    void Start()
    {
        doSetScreen("MainMenuPanel");
    }

    public void doSetScreen(string screen)
    {
        MainMenuPanel.SetActive(screen == "MainMenuPanel");
        SettingPanel.SetActive(screen == "SettingPanel");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNSerttingNMore : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject SettingPanel;
    public GameObject VideoPanel;
    public GameObject AudioPanel;

    // Start is called before the first frame update
    void Start()
    {
        doSetScreen("MainMenuPanel");
    }

    public void doSetScreen(string screen)
    {
        MainMenuPanel.SetActive(screen == "MainMenuPanel");
        SettingPanel.SetActive(screen == "SettingPanel");
        VideoPanel.SetActive(screen == "VideoPanel");
        AudioPanel.SetActive(screen == "AudioPanel");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

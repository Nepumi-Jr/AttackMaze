﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MenuNSerttingNMore : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject SettingPanel;
    public GameObject VideoPanel;
    public GameObject AudioPanel;
    public GameObject ContiPanel;
    public GameObject NewGamePanel;
    //TODO : new Game

    // Start is called before the first frame update

    public void doSetScreen(string screen)
    {
        MainMenuPanel.SetActive(screen == "MainMenuPanel");
        SettingPanel.SetActive(screen == "SettingPanel");
        VideoPanel.SetActive(screen == "VideoPanel");
        AudioPanel.SetActive(screen == "AudioPanel");
        NewGamePanel.SetActive(screen == "NewGamePanel");
        ContiPanel.SetActive(false);
    }

    public void openUpTheSky()
    {
        MainMenuPanel.SetActive(true);
        SettingPanel.SetActive(true);
        VideoPanel.SetActive(true);
        AudioPanel.SetActive(true);
        ContiPanel.SetActive(true);
        NewGamePanel.SetActive(true);
    }

    public void gameCall()
    {
        if(File.Exists(Application.dataPath + "/data.fgm"))
        {
            ContiPanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }
        else
        {
            NewGamePanel.SetActive(true);
            MainMenuPanel.SetActive(false);
        }
    }

    public void gameContinue()
    {
        if(GameDataManager.getPhase() == "Solving")
        {
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.SolveMaze);
        }
        else
        {
            ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.ContructMaze);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

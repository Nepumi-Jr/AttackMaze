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
    public GameObject HowtoPlay;
    public BGMManager BGM;
    bool ff = false;
    public GameObject fader;

    // Start is called before the first frame update
    private void Update()
    {
        if (!ff)
        {
            ff = true;
            BGM.startPlaying();
        }
        
    }

    public void doSetScreen(string screen)
    {
        MainMenuPanel.SetActive(screen == "MainMenuPanel");
        SettingPanel.SetActive(screen == "SettingPanel");
        VideoPanel.SetActive(screen == "VideoPanel");
        AudioPanel.SetActive(screen == "AudioPanel");
        NewGamePanel.SetActive(screen == "NewGamePanel");
        HowtoPlay.SetActive(screen == "HowToPlay");
        ContiPanel.SetActive(false);
        BGM.fadeVolume((screen == "SettingPanel" || screen == "VideoPanel" || screen == "HowToPlay") ? 0.6f : 1f, 2f);
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
        BGM.fadeVolume(0f, 20f);
        if (GameDataManager.getPhase() == "Solving")
        {
            StartCoroutine(startTran(ScreenLoadManager.Scene.SolveMaze));
        }
        else
        {
            StartCoroutine(startTran(ScreenLoadManager.Scene.ContructMaze));
        }
    }

    public void gameNew()
    {
        BGM.fadeVolume(0f, 20f);
        StartCoroutine(startTran(ScreenLoadManager.Scene.ContructMaze));
    }

    IEnumerator startTran(ScreenLoadManager.Scene x)
    {
        fader.SetActive(true);
        yield return new WaitForSeconds(2f);
        ScreenLoadManager.loadNextScreen(x);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}

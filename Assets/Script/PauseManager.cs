using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    //Contruct Control
    [HideInInspector] public bool isPause = false;
    public GameObject pauseObject;
    public BGMManager BgmMain;

    public void callPause()
    {
        isPause = !isPause;
        BgmMain.fadeVolume(isPause ? 0.2f : 1, 1.5f);
        pauseObject.SetActive(isPause);

    }

    public void backToMenu()
    {
        ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.MainMenu);
    }
}

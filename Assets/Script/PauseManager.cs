using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    //Contruct Control
    [HideInInspector] public bool isPause = false;
    public GameObject pauseObject;

    public void callPause()
    {
        isPause = !isPause;
        pauseObject.SetActive(isPause);

    }

    public void backToMenu()
    {
        ScreenLoadManager.loadNextScreen(ScreenLoadManager.Scene.MainMenu);
    }
}

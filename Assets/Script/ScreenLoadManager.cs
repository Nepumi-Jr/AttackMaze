using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScreenLoadManager
{
    private static AsyncOperation LoadingAO;

    private class LoadingMonoBehaviour : MonoBehaviour
    {

    }

    public enum Scene
    {
        MainMenu, SolveMaze, Loading
    }

    private static Action OnLoaderCallback;

    public static void loadNextScreen(Scene nextScreen)
    {

        OnLoaderCallback = () =>
        {
            GameObject LGame = new GameObject("Loading Game Object");
            LGame.AddComponent<LoadingMonoBehaviour>().StartCoroutine(
            LoadSA(nextScreen));
        };
        SceneManager.LoadScene(Scene.Loading.ToString());


    }

    private static IEnumerator LoadSA(Scene SS)
    {
        yield return null;
        LoadingAO = SceneManager.LoadSceneAsync(SS.ToString());

        while (!LoadingAO.isDone)
        {
            yield return null;
        }

    }


    public static float progress()
    {
        if (LoadingAO != null)
        {
            return LoadingAO.progress;
        }
        else return 1f;
    }

    public static void LoadCallIsla()
    {
        if (OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }
}

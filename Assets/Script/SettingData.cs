using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SettingData
{
    public static int resolutionIndex = 0;
    public static int graphicQuality = 0;
    public static bool isFullScreen = true;

    public static float masterVolume = 1f;
    public static float musicVolume = 1f;
    public static float soundVolume = 1f;

    public static string langSelected = "EN";

    public static void saveSetting()
    {
        SettingDataForObject temp = new SettingDataForObject();
        string filePath = Application.dataPath + "/setting.json";
        temp.vf = SettingData.isFullScreen;
        temp.vr = SettingData.resolutionIndex;
        temp.vq = SettingData.graphicQuality;
        temp.mms = SettingData.musicVolume;
        temp.mmt = SettingData.masterVolume;
        temp.msf = SettingData.soundVolume;
        temp.ls = SettingData.langSelected;
        string jsonContent = JsonUtility.ToJson(temp);


        File.WriteAllText(filePath, jsonContent);
    }

    public static void loadSetting()
    {
        
        string filePath = Application.dataPath + "/setting.json";

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            SettingDataForObject holder = JsonUtility.FromJson<SettingDataForObject>(jsonContent);


            SettingData.isFullScreen = holder.vf;
            SettingData.resolutionIndex = holder.vr;
            SettingData.graphicQuality = holder.vq;
            SettingData.musicVolume = holder.mms;
            SettingData.masterVolume = holder.mmt;
            SettingData.soundVolume = holder.msf;
            SettingData.langSelected = holder.ls;
        }
        else
        {
            //File not found

            Resolution[] reses = Screen.resolutions;

            isFullScreen = Screen.fullScreen;
            resolutionIndex = 0;

            for (int i = 0; i < reses.Length; i++)
            {
                if(reses[i].width == Screen.width && reses[i].height == Screen.height
                    &&reses[i].refreshRate == Screen.currentResolution.refreshRate)
                {
                    resolutionIndex = i;
                    break;
                }
            }

            graphicQuality = QualitySettings.GetQualityLevel();

            saveSetting();

        }

        
        applyAll();
    }


    public static string toString()
    {
        string strContent = "";
        strContent += string.Format("Video : {0}, FS = {1}, Qua = {2}\n",
            resolutionIndex, isFullScreen, graphicQuality);
        strContent += string.Format("Audio : Master = {0}, Music = {1}, SFX = {2}\n",
            masterVolume, musicVolume, soundVolume);
        strContent += string.Format("Language : {0}",
            langSelected);

        return strContent;
    }

    public static void applyVideo()
    {
        Resolution[] reses = Screen.resolutions;

        QualitySettings.SetQualityLevel(graphicQuality);
        Screen.SetResolution(reses[resolutionIndex].width, reses[resolutionIndex].height,
            isFullScreen);
        saveSetting();
    }

    public static void applySound()
    {
        AudioMixer audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
        audioMixer.SetFloat("Master", (Mathf.Pow(40, (1f - masterVolume) - 1f) * (-80)));
        audioMixer.SetFloat("Music", (Mathf.Pow(40, (1f - musicVolume) - 1f) * (-80)));
        audioMixer.SetFloat("SFX", (Mathf.Pow(40, (1f - soundVolume) - 1f) * (-80)));
        saveSetting();
    }

    public static void nextLang()
    {
        langSelected = LangManager.moveOnSet(langSelected);
        saveSetting();
    }

    public static void applyAll()
    {
        LangManager.loadLang(langSelected);
        applyVideo();
        applySound();
        LangManager.init();

        GameObject[] gameO = GameObject.FindGameObjectsWithTag("ReloadLang");
        foreach (GameObject e in gameO)
        {
            ReloadLangNow r;
            if (e.TryGetComponent(out r))
            {
                r.ReloadText();
            }
            ReloadLangMeshNow rm;
            if (e.TryGetComponent(out rm))
            {
                rm.ReloadText();
            }
        }

    }


}

[System.Serializable]
public class SettingDataForObject
{
    public int vr;
    public bool vf;
    public int vq;

    public float mmt;
    public float mms;
    public float msf;

    public string ls;

}


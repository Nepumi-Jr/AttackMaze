using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Debuggg : MonoBehaviour
{
    Text ppp;

    // Start is called before the first frame update
    void Start()
    {
        AudioMixer audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
        if (GameDataManager.isDebuging)
        {
            float res;
            audioMixer.GetFloat("Music", out res);
            ppp = GetComponent<Text>();
            ppp.text = string.Format("Debug!\nSetting music = {0}\nreal mixer = {1}\n{2}",SettingData.musicVolume, res, SettingData.toString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

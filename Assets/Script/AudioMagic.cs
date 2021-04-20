using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMagic : MonoBehaviour
{

    AudioSource audioSource;

    public bool isMusic = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SettingData.applySound(false);
        AudioMixer audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(isMusic ? "Music" : "SFX")[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

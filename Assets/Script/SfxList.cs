using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxList : MonoBehaviour
{
    public List<string> sfxKey;
    public List<AudioClip> sfxClip;

    Dictionary<string, AudioClip> dict = new Dictionary<string, AudioClip>();
    AudioSource sfxAS;

    // Start is called before the first frame update
    void Start()
    {
        sfxAS = this.GetComponent<AudioSource>();
        for(int i = 0; i < Mathf.Min(sfxClip.Count, sfxKey.Count); i++)
        {
            dict.Add(sfxKey[i], sfxClip[i]);
        }
    }

    public void playSfx(string key)
    {
        AudioClip Yeah;
        if (dict.TryGetValue(key, out Yeah))
        {
            sfxAS.PlayOneShot(Yeah);
        }
        else
        {
            Debug.LogWarning(key + "Sound not found :(");
        }
    }
}

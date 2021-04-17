using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSfx : MonoBehaviour
{
    AudioSource sfxSource;
    public AudioClip click;
    public AudioClip confirm;

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    public void playClick(bool isConfirm)
    {
        sfxSource.PlayOneShot(isConfirm ? confirm : click);
    }

}

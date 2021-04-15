using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip mainStart;
    public AudioClip mainLoop;

    public ContructBigControl CBC;


    AudioSource mainAS;
    AudioSource loopAS;

    bool isPlayed = false;
    float bigVol = 1f;
    float bigSpeed = 1f;
    float timWait = 0f;
    bool isLoopPlayed = true;

    public float[] beatSample = new float[8];

    public void startPlaying(float offset = 0f)
    {
        if (!isPlayed)
        {
            isPlayed = true;
            mainAS = transform.GetChild(0).GetComponent<AudioSource>();
            loopAS = transform.GetChild(1).GetComponent<AudioSource>();

            loopAS.loop = true;
            loopAS.clip = mainLoop;
            mainAS.loop = false;

            mainAS.PlayOneShot(mainStart);
            timWait = mainStart.length - offset - 0.1f;
            isLoopPlayed = false;
            
        }
        
    }


    public void fadeVolume(float newVol = 1f,float speed = -1f)
    {
        bigVol = newVol;
        if(speed > 0f)
        {
            bigSpeed = speed;
        }
    }



    private void Update()
    {

        if (isPlayed)
        {
            if (bigVol != mainAS.volume)
            {
                loopAS.volume = Mathf.Lerp(mainAS.volume, bigVol, Time.deltaTime * bigSpeed);
                mainAS.volume = Mathf.Lerp(mainAS.volume, bigVol, Time.deltaTime * bigSpeed);

            }
        }

        if (!isLoopPlayed)
        {
            if(timWait - Time.deltaTime >= 0f)
            {
                timWait -= Time.deltaTime;
            }
            else
            {
                loopAS.Play();
                isLoopPlayed = true;
            }
        }


    }

    public void ChangeSongAndPlay(AudioClip startClip,AudioClip loopClip)
    {
        mainAS.Stop();
        loopAS.Stop();

        mainLoop = loopClip;
        mainStart = startClip;

        loopAS.loop = true;
        loopAS.clip = mainLoop;
        mainAS.loop = false;

        mainAS.PlayOneShot(mainStart);
        timWait = mainStart.length;
        isLoopPlayed = false;
    }

}

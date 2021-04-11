using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip mainStart;
    public AudioClip mainLoop;

    public ContructBigControl CBC;


    AudioSource mainAS;

    bool isPlayed = false;
    float bigVol = 1f;
    float bigSpeed = 1f;
    float tim = 0;

    public float[] beatSample = new float[1];

    public void startPlaying()
    {
        if (!isPlayed)
        {
            isPlayed = true;
            mainAS = transform.GetChild(0).GetComponent<AudioSource>();
            mainAS.PlayOneShot(mainStart);
            mainAS.loop = false;

            StartCoroutine(startLoop(mainStart.length));
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


    private IEnumerator startLoop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mainAS.loop = true;
        mainAS.clip = mainLoop;

        mainAS.Play();
    }

    private void Update()
    {

        float opa = 0f;
        if (isPlayed)
        {
            if (bigVol != mainAS.volume)
            {
                mainAS.volume = Mathf.Lerp(mainAS.volume, bigVol, Time.deltaTime * bigSpeed);
                Debug.Log(mainAS.volume);
            }

            if (QualitySettings.GetQualityLevel() >= 1)
            {
                mainAS.GetOutputData(beatSample, 0);
                float newOpa = Mathf.Clamp01(Mathf.Log(Mathf.Max(beatSample[0] / 0.1f, 0f)) + 1);
                opa = newOpa;
            }

            

        }
        if (QualitySettings.GetQualityLevel() == 1)
        {
            tim += Time.deltaTime;
            tim %= 2 * Mathf.PI;

            opa = Mathf.Sin(tim);
        }

        if (CBC != null)
        {
            CBC.setLightOpaci(opa);
        }


    }

}

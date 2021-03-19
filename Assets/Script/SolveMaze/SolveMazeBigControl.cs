using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveMazeBigControl : MonoBehaviour
{

    float timtick = 0f;
    AudioSource audioSource;
    public AudioClip BGM;

    public EachPlayer p1Field;
    public EachPlayer p2Field;

    float tim = 0f;
    public float[] beatSample = new float[1];
    float nowOpa = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = BGM;
        audioSource.Play();

        GameDataManager.loadGame();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetOutputData(beatSample, 0);
        
        tim += Time.deltaTime;
        tim %=  2 * Mathf.PI;

        float newOpa = Mathf.Clamp01(Mathf.Log(Mathf.Max(beatSample[0] / 0.1f,0f)) + 1);
        nowOpa = Mathf.Lerp(nowOpa, newOpa, 0.2f);

        p1Field.setLightOpaci(nowOpa);

    }
}

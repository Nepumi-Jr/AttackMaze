using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{

    Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = Mathf.Lerp(progressBar.value, ScreenLoadManager.progress(), 
            Time.deltaTime*10f);
    }
}

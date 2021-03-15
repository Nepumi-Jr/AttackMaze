using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUseLang : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text thisText = this.GetComponent<Text>();

        thisText.text = LangManager.calling("TestText1");
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

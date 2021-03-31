using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCalling : MonoBehaviour
{

    public string codeName;
    public bool isTitle = false;
    bool FF = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!FF)
        {
            GetComponent<Text>().text = LangManager.calling(codeName);
            GetComponent<Text>().font = isTitle ? LangManager.titleTextFont : LangManager.textFont;
            FF = true;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCalling : MonoBehaviour
{

    public string codeName;
    bool FF = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!FF)
        {
            GetComponent<Text>().text = LangManager.calling(codeName);
            FF = true;
        }
    }
}

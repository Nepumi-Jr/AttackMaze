using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCalling : MonoBehaviour
{

    public string codeName;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = LangManager.calling(codeName);
    }
}

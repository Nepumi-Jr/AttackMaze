using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadLangNow : MonoBehaviour
{
    public string codeLang = "";
    Text thisText;

    public void ReloadText()
    {
        thisText = GetComponent<Text>();
        if (thisText == null)
        {
            Debug.LogError("GRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR + " + this.name);
        }
        thisText.text = LangManager.calling(codeLang);
    }
}

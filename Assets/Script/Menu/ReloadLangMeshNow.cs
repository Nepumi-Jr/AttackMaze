using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReloadLangMeshNow : MonoBehaviour
{
    public string codeLang = "";
    public bool isTitle = false;
    TextMeshProUGUI thisText;
    
    public void ReloadText()
    {
        thisText = GetComponent<TextMeshProUGUI>();
        if (thisText == null)
        {
            Debug.LogError("GRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR + " + this.name);
        }
        thisText.text = LangManager.calling(codeLang);
        thisText.font = isTitle ? TMP_FontAsset.CreateFontAsset(LangManager.titleTextFont) : TMP_FontAsset.CreateFontAsset(LangManager.textFont);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextMeshCalling : MonoBehaviour
{

    public string codeName;
    public bool isTitle = false;
    bool FF = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!FF)
        {
            GetComponent<TextMeshProUGUI>().text = LangManager.calling(codeName);
            GetComponent<TextMeshProUGUI>().font = isTitle ? TMP_FontAsset.CreateFontAsset(LangManager.titleTextFont) : TMP_FontAsset.CreateFontAsset(LangManager.textFont);
            FF = true;
        }
        
    }
}

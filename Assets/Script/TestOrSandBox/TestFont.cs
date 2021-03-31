using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestFont : MonoBehaviour
{

    TextMeshProUGUI tm;
    

    // Start is called before the first frame update
    void Start()
    {
        tm = this.GetComponent<TextMeshProUGUI>();
        tm.font = TMP_FontAsset.CreateFontAsset(Resources.Load<Font>("Langs/TH_Title"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

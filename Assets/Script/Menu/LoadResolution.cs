using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadResolution : MonoBehaviour
{
    Resolution[] resolutions;
    Dropdown dropdownBruh;
    List<Dropdown.OptionData> datas;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        dropdownBruh = this.GetComponent<Dropdown>();
        datas = new List<Dropdown.OptionData>();
        foreach (Resolution res in resolutions)
        {
            datas.Add(new Dropdown.OptionData(res.ToString()));
        }
        dropdownBruh.ClearOptions();
        dropdownBruh.AddOptions(datas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

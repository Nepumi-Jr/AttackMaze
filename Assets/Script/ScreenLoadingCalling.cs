using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLoadingCalling : MonoBehaviour
{
    bool FF = false;

    private void Update()
    {
        if (!FF)
        {
            FF = true;
            ScreenLoadManager.LoadCallIsla();
        }
    }

}

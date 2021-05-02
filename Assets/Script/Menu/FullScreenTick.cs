using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenTick : MonoBehaviour
{

    public Text buttonText;
    bool isPress = false;
    Button thisButton;

    private void Start()
    {
        thisButton = GetComponent<Button>();
    }

    public void Bhit()
    {
        isPress = !isPress;
        SettingData.isFullScreen = isPress;
        SettingData.applyVideo();
        buttonText.text = (isPress) ? "/" : " ";
    }

    public void doSet(bool full)
    {
        isPress = full;
        buttonText.text = (isPress) ? "/" : " ";
    }

    public void setInteractable(bool interact)
    {
        thisButton.interactable = interact;
    }
}

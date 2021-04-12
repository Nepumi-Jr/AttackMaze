using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCallingAndListener : MonoBehaviour
{

    public Dropdown VRes;
    public FullScreenTick VFul;
    public Dropdown VQua;

    public Slider MMas;
    public Slider MMus;
    public Slider MSfx;

    public Text langText;

    public Text phaseText;
    public Text lastseenText;


    // Start is called before the first frame update
    void Start()
    {
        SettingData.loadSetting();

        VRes.value = SettingData.resolutionIndex;
        VFul.doSet(SettingData.isFullScreen);
        VQua.value = SettingData.graphicQuality;

        MMas.value = SettingData.masterVolume;
        MMus.value = SettingData.musicVolume;
        MSfx.value = SettingData.soundVolume;

        langText.text = "Lang : " + LangManager.calling("LangNameNative");
        langText.font = LangManager.textFont;

        lastseenText.text = GameDataManager.lastSeen();
        phaseText.text = LangManager.calling("P" + GameDataManager.getPhase());

        this.GetComponent<MenuNSerttingNMore>().doSetScreen("MainMenuPanel");

    }

    public void vChangeResolution(int ind)
    {
        SettingData.resolutionIndex = ind;
        SettingData.applyVideo();
    }

    public void vChangeFullscreen(bool tog)
    {
        SettingData.isFullScreen = tog;
        SettingData.applyVideo();
    }

    public void vChangeQuality(int indQ)
    {
        SettingData.graphicQuality = indQ;
        SettingData.applyVideo();
    }

    public void aChangeMaster(float value)
    {
        SettingData.masterVolume = value;
        SettingData.applySound();
    }
    public void aChangeMusic(float value)
    {
        SettingData.musicVolume = value;
        SettingData.applySound();
    }

    public void aChangeSFX(float value)
    {
        SettingData.soundVolume = value;
        SettingData.applySound();
    }
    public void doLangChange()
    {
        SettingData.nextLang();
        reloadsLang();
    }

    public void reloadsLang()
    {
        this.GetComponent<MenuNSerttingNMore>().openUpTheSky();
        GameObject[] gameO = GameObject.FindGameObjectsWithTag("ReloadLang");
        foreach (GameObject e in gameO)
        {
            ReloadLangNow r;
            if (e.TryGetComponent(out r))
            {
                r.ReloadText();
            }
            ReloadLangMeshNow rm;
            if (e.TryGetComponent(out rm))
            {
                rm.ReloadText();
            }
        }
        langText.text = "Lang : " + LangManager.calling("LangNameNative");
        langText.font = LangManager.textFont;
        this.GetComponent<MenuNSerttingNMore>().doSetScreen("SettingPanel");
        phaseText.text = LangManager.calling("P" + GameDataManager.getPhase());
    }

}

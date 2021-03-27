using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangManager
{
    protected static Dictionary<string, string> langData = new Dictionary<string, string>();
    protected static string selectedLang;
    protected static List<string> avaliableLang = new List<string>();

    private static bool firstTime = false;

    public LangManager()
    {
        init();
    }

    public static void init()
    {
        if (!firstTime)
        {
            avaliableLang.Clear();
            firstTime = true;
            foreach (TextAsset eachLang in Resources.LoadAll<TextAsset>("Langs"))
            {
                avaliableLang.Add(eachLang.name);
            }
            loadLang("EN");
        }
    }

    public static void loadLang(string lang)
    {
        init();
        selectedLang = lang;
        langData.Clear();

        string fileText = Resources.Load<TextAsset>("Langs/EN").text;
        TextAsset file = Resources.Load<TextAsset>("Langs/" + selectedLang);
        if (file != null)
        {
            fileText = file.text;
        }
        else
        {
            Debug.LogWarning(selectedLang + "NOT FOUND");
        }

        string nameData = "";
        string textData = "";

        foreach (string line in fileText.Trim().Split('\n'))
        {
            if (line.StartsWith("["))
            {
                if (nameData != "")
                {
                    langData.Add(nameData, textData.Trim());
                    textData = "";
                }

                nameData = line.Substring(1, line.IndexOf(']') - line.IndexOf('[') - 1);

            }
            else
            {
                textData += line + "\n";
            }
        }

        if (nameData != "")
        {
            langData.Add(nameData, textData.Trim());
            textData = "";
        }

    }

    public static string calling(string name)
    {
        init();
        if (langData.ContainsKey(name))
        {
            return langData[name];
        }
        Debug.LogWarning("missing " + name);
        return "missing " + name;
    }

    public static string moveOnSet(string curLang)
    {
        TextAsset[] allLang =  Resources.LoadAll<TextAsset>("Langs");
        int ind = -1;
        for(int i = 0; i < allLang.Length; i++)
        {
            if(allLang[i].name == curLang)
            {
                ind = i;
                break;
            }
        }
        ind++;
        ind %= allLang.Length;

        loadLang(allLang[ind].name);

        return allLang[ind].name;

    }
}

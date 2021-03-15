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
            Debug.Log("Hello");
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

        string fileText = Resources.Load<TextAsset>("Langs/" + selectedLang).text;

        string nameData = "";
        string textData = "";

        foreach (string line in fileText.Trim().Split('\n'))
        {
            if (line.StartsWith("["))
            {
                if (nameData != "")
                {
                    langData.Add(nameData, textData.Trim());
                    Debug.Log("Adding " + nameData + "...\n" + textData.Trim());
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
            Debug.Log("Adding " + nameData + "...\n" + textData.Trim());
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem
{
    public enum Language
    {
        English,
        Vietnamese
    }

    public static Language language = Language.English;

    private static Dictionary<string, string> localizedEN;
    private static Dictionary<string, string> localizedVI;

    public static bool isInit;

    public static void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localizedEN = csvLoader.GetDictionaryValues("en");
        localizedVI = csvLoader.GetDictionaryValues("vi");

        isInit = true;
    }

    public static string GetLocalizedValue(string key)
    {
        if (!isInit) { Init(); }

        string value = key;
        switch(language)
        {
            case Language.English:
                localizedEN.TryGetValue(key, out value);
                break;
            case Language.Vietnamese:
                localizedVI.TryGetValue(key, out value);
                break;
        }

        return value;
    }
}

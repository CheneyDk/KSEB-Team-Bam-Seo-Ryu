using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RE_SaveLoader
{
    public static void Save<T>(T data) where T : RE_SaveData
    {
        if (!Directory.Exists(data.GetDirectory()))
        {
            Directory.CreateDirectory(data.GetDirectory());
        }

        var json = JsonUtility.ToJson(data);
        File.WriteAllText(data.GetFullPath(), json);
    }

    public static T Load<T>(string fileName, string dir) where T : RE_SaveData
    {
        var fullPath = Application.persistentDataPath + "/" + dir + "/" + fileName + ".json";

        if (!File.Exists(fullPath))
        {
            return null;
        }

        var json = File.ReadAllText(fullPath);
        var data = JsonUtility.FromJson<T>(json);

        return data;
    }
}
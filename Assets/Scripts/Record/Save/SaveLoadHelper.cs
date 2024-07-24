using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadHelper
{
    public static void Save(RecordData data)
    {
        if (!Directory.Exists(data.GetDirectory()))
        {
            Directory.CreateDirectory(data.GetDirectory());
        }

        var json = JsonUtility.ToJson(data);
        File.WriteAllText(data.GetFullPath(), json);

        //Debug.Log("Data is Saved : " + data.GetFullPath());
    }

    public static RecordData Load(string fileName, string dir)
    {
        var fullPath = Application.persistentDataPath + "/" + dir + "/" + fileName + ".json";

        if (!File.Exists(fullPath))
        {
            //Debug.LogError("There is no file : " + fullPath);
            return null;
        }

        var json = File.ReadAllText(fullPath);
        var data = JsonUtility.FromJson<RecordData>(json);

        //Debug.Log("Data is Loaded : " + fullPath);

        return data;
    }
}
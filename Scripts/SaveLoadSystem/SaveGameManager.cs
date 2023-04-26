using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveGameManager
{
    public static SaveData currentSaveData = new SaveData();
    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";
    public static void Save()
    {
        var dir = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        string json = JsonUtility.ToJson(currentSaveData, true);
        File.WriteAllText(dir + FileName, json);
        GUIUtility.systemCopyBuffer = dir;
    }

    public static void Load()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            currentSaveData = JsonUtility.FromJson<SaveData>(json);
            return;
        }
        currentSaveData = new SaveData();
    }

    public static void Flush()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        currentSaveData = new SaveData();
    }
}

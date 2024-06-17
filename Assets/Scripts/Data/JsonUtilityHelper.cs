using System.IO;
using UnityEngine;

public static class JsonUtilityHelper
{
    public static void SaveToJson<T>(T data, string filePath)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static T LoadFromJson<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return default;
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(json);
    }

    public static void LoadFromJsonOverwrite<T>(string filePath, T data)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, data);
    }
}

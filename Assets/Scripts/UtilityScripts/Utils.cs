 using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public static class Utils
{
    private static int sceneToLoad;

    public static readonly float fadeTime = 1f;
    private static readonly string saveFolder = "/SaveData/";
    private static readonly string savePath = Application.dataPath + saveFolder;

    public static float Rand(Vector2 range) => Random.Range(range.x, range.y);

    public static string EnumInt<T>(T enumValue) {
        return (System.Convert.ToInt32(enumValue)).ToString();
    }

    public static T IntEnum<T>(string intString)
    {
        var num = int.Parse(intString);
        return (T) (object) num;
    }

    public static List<T> EnumToList<T>()
    {
        var array = System.Enum.GetValues(typeof(T));
        var list = new List<T>();
        foreach (var item in array) list.Add((T)item);
        return list;
    }

    public static void SaveToFile(string fileName, string text)
    {
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        var file = File.CreateText(savePath + fileName);
        file.Write(text);
        file.Close();
    }

    public static string ReadFromFile(string fileName)
    {
        var completePath = savePath + fileName;
        if (File.Exists(completePath)) return File.ReadAllText(completePath);
        return "";
    }

    public static string GetTimeString(int seconds)
    {
        seconds = Mathf.FloorToInt(seconds);
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(seconds);
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        return timeString;
    }

    private static void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public static void SetDirty(Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }


    public static string Vec2String(Vector2 v) => v.x + ":" + v.y;
}

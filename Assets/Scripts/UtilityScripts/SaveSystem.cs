using MyBox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// Handles saving and loading data to and from files, as well as loading save strings by ID for characters.
/// </summary>
public static class SaveSystem
{
    private static readonly string saveFolder = "/SaveData/";
    public static readonly string dynamicDataFileName = "dynamicData.txt";
    public static readonly string staticDataFileName = "characters.txt";
    public static readonly string relationshipsFileName = "relationships.txt";
    private static readonly string savePath = Application.streamingAssetsPath + saveFolder;
    public static int IDLength = 4;

    public static void SaveDynamicData(string dynamicData)
    {
        var targetID = dynamicData.Split('~')[0];

        var saveStrings = ReadFromFile(dynamicDataFileName).Split('\n').Where(x => x.Length > 1).ToList();
        for (int i = 0; i < saveStrings.Count; i++) {

            var ID = saveStrings[i].Split('~')[0];
            if (ID == targetID) {
                saveStrings[i] = dynamicData;
                SaveToFile(dynamicDataFileName, string.Join("\n", saveStrings));
                return;
            }
        }

        saveStrings.Add(dynamicData);
        SaveToFile(dynamicDataFileName, string.Join("\n", saveStrings));
    }

    /// <summary>
    /// Helper to save text to a file in the saves directory.
    /// Creates a file if none exists.
    /// </summary>
    public static void SaveToFile(string fileName, string text)
    {
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        var file = File.CreateText(savePath + fileName);
        file.Write(text);
        file.Close();

        //Debug.Log("successfully wrote to " + savePath + fileName);
    }

    /// <summary>
    /// Helper to read text from a file in the saves directory.
    /// returns "" if no file exists.
    /// </summary>
    public static string ReadFromFile(string fileName)
    {
        var completePath = savePath + fileName;
        if (File.Exists(completePath)) return File.ReadAllText(completePath);
        return "";
    }

    /// <summary>
    /// Given an ID, returns the static saveString for that character from the static data file.
    /// returns "" if no character with that ID exists in the save file.
    /// </summary>
    public static string GetStaticSaveString(ID ID)
    {
        var saves = LoadAllStaticSaveStrings();
        foreach (var save in saves) {
            if (save[..IDLength] == ID) {
                return save;
            }
        }
        return "";
    }

    /// <summary>
    /// Returns the static saveString for a random character from the static data file.
    /// </summary>
    public static string GetRandomStaticSaveString()
    {
        var savedText = File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, staticDataFileName));
        var saveStrings = savedText.Split('\n').Where(x => x.Length > 0).ToList();
        return saveStrings[Random.Range(0, saveStrings.Count)];
    }

    /// <summary>
    /// Returns the static data for all saved characters, shuffled, limited to the given quantity.
    /// </summary>
    public static List<string> GetRandomStaticSaveStrings(int quantity)
    {
        var saveStrings = LoadAllStaticSaveStrings().Shuffle();
        quantity = Mathf.Min(quantity, saveStrings.Count);
        return saveStrings.Take(quantity).ToList();
    }

    /// <summary>
    /// Returns a list of all the 'static' saveStrings for characters, generated from character creator.
    /// </summary>
    public static List<string> LoadAllStaticSaveStrings()
    {
        var savedText = File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, staticDataFileName));
        var saveStrings = savedText.Split('\n').Where(x => x.Length > IDLength).ToList();

        return saveStrings;
    }

    /// <summary>
    /// Given an ID, loads and returns the portrait Sprite for that character.
    /// </summary>
    public static Sprite GetPortrait(ID ID)
    {
        var fileName = ID + "_portrait.png";
        var path = Application.streamingAssetsPath + "/ID_images/" + fileName;
        if (!File.Exists(path)) {
            //Debug.Log("Path not found: " + path);
            return null;
        }

        var bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        texture.filterMode = FilterMode.Bilinear;

        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        return sprite;
    }

    /// <summary>
    /// Calculates age in years based on the given birthday compared to irl time right now.
    /// </summary>
    public static int GetAge(System.DateTime birthday)
    {
        System.DateTime today = System.DateTime.Now;

        int age = today.Year - birthday.Year;

        if (today < birthday.AddYears(age))
            age--;

        return age;
    }
}

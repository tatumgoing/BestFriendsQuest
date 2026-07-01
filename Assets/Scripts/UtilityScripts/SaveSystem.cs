using MyBox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles saving and loading data to and from files, as well as loading save strings by ID for characters.
/// </summary>
public static class SaveSystem
{
    private static readonly string saveFolder = "/SaveData/";
    public static readonly string dynamicDataFileName = "dynamicData.txt";
    public static readonly string relationshipFileName = "relationships.txt";
    public static readonly string inProgressBFQuestFileName = "inProgressBFQuests.txt";
    public static readonly string completedBFQuestFileName = "completedBFQuests.txt";
    public static readonly string highscoreFileName = "highscores.txt";
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

    public static void ResetCompletedBFQuests()
    {
        SaveToFile(completedBFQuestFileName, "");
    }

    public static void SaveCompletedBFQuest(Quest quest)
    {
        var text = ReadFromFile(completedBFQuestFileName);
        if (text.ContainsInsensitive(quest.name)) return;

        text += quest.name + "\n";
        SaveToFile(completedBFQuestFileName, text);
    }

    public static List<ID> GetAllQuestingCharacters()
    {
        var IDs = new List<ID>();
        var savedQuests = ReadFromFile(inProgressBFQuestFileName).Split("\n").Where(x => x.Length > 2).ToList();
        foreach (var q in savedQuests) {
            var loadedQuest = new RuntimeQuestData();
            loadedQuest.LoadFromSaveString(q);
            if (!IDs.Contains(loadedQuest.Character1)) IDs.Add(loadedQuest.Character1);
            if (!IDs.Contains(loadedQuest.Character2)) IDs.Add(loadedQuest.Character2);
        }

        return IDs;
    }

    public static void DeleteSavedQuest(Quest quest)
    {
        var questStrings = new List<string>();
        var savedQuests = ReadFromFile(inProgressBFQuestFileName).Split("\n").Where(x => x.Length > 2).ToList();
        foreach (var q in savedQuests) {
            if (q.ContainsInsensitive(quest.name)) continue;
            questStrings.Add(q);
        }

        SaveToFile(inProgressBFQuestFileName, string.Join("\n", questStrings));
    }

    public static void SaveBFQuest(RuntimeQuestData quest)
    {
        var questStrings = new List<string>();
        var savedQuests = ReadFromFile(inProgressBFQuestFileName).Split("\n").Where(x => x.Length > 2).ToList();
        foreach (var q in savedQuests) {
            if (q.ContainsInsensitive(quest.QuestData.name)) continue;
            questStrings.Add(q);
        }
        questStrings.Add(quest.GetSaveString());

        SaveToFile(inProgressBFQuestFileName, string.Join("\n", questStrings));

        //Debug.Log("Saved quest: " + quest.QuestData.name);
    }

    public static RuntimeQuestData LoadBFQuest(List<Quest> quests)
    {
        var questData = new RuntimeQuestData();
        var savedQuests = ReadFromFile(inProgressBFQuestFileName).Split("\n").Where(x => x.Length > 2).ToList();
        foreach (var savedQuest in savedQuests) {
            foreach (var q in quests) {
                if (savedQuest.ContainsInsensitive(q.name)) {
                    questData.LoadFromSaveString(savedQuest);
                    questData.QuestData = q;
                    return questData;
                }
            }
        }

        return null;
    }

    public static RuntimeQuestData LoadBFQuest(Quest quest)
    {
        var questData = new RuntimeQuestData();
        var savedQuests = ReadFromFile(inProgressBFQuestFileName).Split("\n").Where(x => x.Length > 2).ToList();
        foreach (var q in savedQuests) {
            if (q.ContainsInsensitive(quest.name)) {
                questData.LoadFromSaveString(q);
                questData.QuestData = quest;
                return questData;
            }
        }

        return null;
    }

    public static void SaveRelationship(ID id1, ID id2, float value)
    {
        var relationships = ReadFromFile(relationshipsFileName).Split("\n").Where(x => x.Length > 2).ToList();

        bool found = false;
        for (int i = 0; i < relationships.Count; i++) {

            var parts = relationships[i].Split(",");
            var loadedID1 = int.Parse(parts[0]);
            var loadedID2 = int.Parse(parts[1]);

            if (loadedID1 == id1 && loadedID2 == id2) {
                parts[2] = value.ToString();
                found = true;
                relationships[i] = string.Join(",", parts);
                break;
            }
        }
        if (!found) relationships.Add(id1 + "," +  id2 + "," + value);

        SaveToFile(relationshipsFileName, string.Join("\n", relationships));
    }

    public static float LoadRelationship(ID id1, ID id2)
    {
        var higherID = Mathf.Max(id1, id2);
        var lowerID = Mathf.Min(id1, id2);
        id1 = new ID(higherID);
        id2 = new ID(lowerID);

        var relationships = ReadFromFile(relationshipsFileName).Split("\n").Where(x => x.Length > 2).ToList();

        for (int i = 0; i < relationships.Count; i++) {
            var parts = relationships[i].Split(",");
            var loadedID1 = int.Parse(parts[0]);
            var loadedID2 = int.Parse(parts[1]);

            if (loadedID1 == id1 && loadedID2 == id2) {
                return float.Parse(parts[2]);
            }
        }
        
        return 0;
    }

    /// <summary>
    /// Given a key and a dictionary of string-float pairs, saves the dictionary to the highscores file in the format "key|string1:float1,string2:float2,..."
    /// overrides the data stored at that key in the highscore file.
    /// to reset, call with an empty dictionary.
    /// </summary>
    public static void SaveHighscoreDictionary(string key, Dictionary<string, float> dictionary)
    {
        var output = "";
        foreach (var pair in dictionary) {
            output += pair.Key + ":" + pair.Value + ",";
        }
        output = key + "|" + output;
        SaveToFile(highscoreFileName, output);
    }

    /// <summary>
    /// Loads in the highscore dictionary stored at the given key in the highscores file.
    /// if the dictionary doesn't exist, returns an empty dictionary.
    /// </summary>
    public static Dictionary<string, float> LoadHighscoreDictionary(string key)
    {
        var lines = ReadFromFile(highscoreFileName).Split('\n');

        var results = new Dictionary<string, float>();
        foreach (var line in lines) {
            if (line.Length < 2) continue;

            var parts = line.Split('|');
            if (parts[0] == key) {
                
                var entries = parts[1].Split(',');
                foreach (var e in entries) {
                    if (e.Length < 2) continue;
                    var entryParts = e.Split(':');
                    results.Add(entryParts[0], float.Parse(entryParts[1]));
                }
                break;
            }
        }
        return results;
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

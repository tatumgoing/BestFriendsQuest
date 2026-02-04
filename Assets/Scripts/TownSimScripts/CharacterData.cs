using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class CharacterData 
{
    TownGameManager gameManager;

    [Header("Profile")]
    public string CharacterName;
    public int Age;

    [Range(0,100) ]public float Happiness = 50;
    public Problem CurrentProblem;

    public CharacterRoomModel RoomScript;

    [HideInInspector] public ID ID { get; private set; }

    public void SolveProblem()
    {
        if (CurrentProblem == null) return;
        CurrentProblem = null;
    }


    /// <summary>
    ///  Saves the dynamic data of this character to a file.
    ///  because Icon, Name, Age, etc are all generated from static data, we don't need to save them here.
    ///  We also don't save problem data because that doesn't get saved between sessions.
    /// </summary>
    public void SaveToFile(ID ID)
    {
        var resultString = ID + "~" + Happiness.ToString();
        SaveSystem.SaveDynamicData(resultString);
    }

    public void LoadFromFile(ID ID)
    {
        var saveStrings = SaveSystem.ReadFromFile(SaveSystem.DynamicDataFileName).Split('\n');
        foreach (var  s in saveStrings) {
            if (s.Split('~')[0] == ID) LoadFromString(s);
        }

        SaveToFile(ID);
        this.ID = ID;
    }

    private void LoadFromString(string saveString)
    {
        var parts = saveString.Split("~");
        if (parts.Length < 2) return;

        // Load Happiness
        Happiness = float.Parse(parts[2]);
    }


    /// <summary>
    /// Creates a new instance of CharacterData based on the 'static' data generated in the character creator or loaded from a savefile.
    /// </summary>
    public CharacterData(StaticCharacterData staticData)
    {
        CharacterName = staticData.Name;
        Age = SaveSystem.GetAge(staticData.Birthday);
        LoadFromFile(staticData.ID);
    }


    /// <summary>
    /// Creates a new instance of CharacterData with default values.
    /// </summary>
    public CharacterData() {}

    public void UpdateHappiness(float newHappiness) {
        Happiness= Mathf.Clamp(Happiness + newHappiness, 0f, 100f);
    }
}

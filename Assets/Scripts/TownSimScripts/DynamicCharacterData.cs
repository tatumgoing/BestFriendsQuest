using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class DynamicCharacterData 
{
    [Range(0,100) ]public float Happiness = 50;
    public ProblemData CurrentProblem;
    private List<ItemData> _inventory = new List<ItemData>(); 
     
    [HideInInspector] public ID ID { get; private set; }

    public List<ItemData> Inventory => _inventory;

    public void AddToInventory(ItemData item) => _inventory.Add(item);

    public void SolveProblem()
    {
        CurrentProblem.IsSolved = true;
    }

    /// <summary>
    /// Call to reward the player for completing the problem. 
    /// Also clears the problem from this character
    /// </summary>
    public void GiveProblemRewards()
    {
        TownGameManager.i.ChangeCurrency(CurrentProblem.RewardCurrency);
        CharacterManager.i.IncreaseHappiness(ID, CurrentProblem.RewardHappiness);

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
    private void SaveToFile() => SaveToFile(ID);

    /// <summary>
    /// Given an ID, load character's dynamic data from file.
    /// </summary>
    public void LoadFromFile(ID ID)
    {
        var saveStrings = SaveSystem.ReadFromFile(SaveSystem.dynamicDataFileName).Split('\n');
        foreach (var  s in saveStrings) {
            if (s.Split('~')[0] == ID) LoadFromString(s);
        }

        SaveToFile(ID);
        this.ID = ID;
    }

    /// <summary>
    /// After loading the string for this character from the file,
    /// this method parses the string and loads the data into this instance.
    /// eventually will load in inventory, preferences, and other dynamic data.
    /// relationships are handled via characterManager.
    /// </summary>
    private void LoadFromString(string saveString)
    {
        var parts = saveString.Split("~");
        if (parts.Length < 2) return;

        // Load Happiness
        Happiness = float.Parse(parts[1]);
    }

    /// <summary>
    /// Creates a new instance of CharacterData based on the 'static' data generated in the character creator or loaded from a savefile.
    /// </summary>
    public DynamicCharacterData(ID id)
    {
        LoadFromFile(id);
    }

    /// <summary>
    /// Creates a new instance of CharacterData with default values.
    /// </summary>
    public DynamicCharacterData() {}

    public void UpdateHappiness(float newHappiness) {
        Happiness= Mathf.Clamp(Happiness + newHappiness, 0f, 100f);
        SaveToFile();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Threading.Tasks;
using System;
using MyBox;

[System.Serializable]
public class DynamicCharacterData 
{
    [Range(0,100), SerializeField, ReadOnly] private float _happiness = 50;
    public ProblemData CurrentProblem;
    public float TimeWhenMoved;
    public  AreaName CurrentLocation;
    private List<ItemData> _inventory = new List<ItemData>(); 
     
    [HideInInspector] public ID ID { get; private set; }

    public List<ItemData> Inventory => _inventory;
    public float Happiness => _happiness;   

    public void AddToInventory(ItemData item)
    {
        _inventory.Add(item);
        SaveToFile();

        if (CurrentProblem != null) {
            if (CurrentProblem.Type == ProblemType.SINGLE_ITEM) {
                if (CurrentProblem.SingleItem == item) {
                    SolveProblem();
                }
            }
            else if (CurrentProblem.Type == ProblemType.GENERAL_ITEM) {
                if (CurrentProblem.GeneralItems.Items.Contains(item)) {
                    SolveProblem();
                }
            }
        }
    }

    public string GetInventoryString()
    {
        if (Inventory.Count == 0) return CharacterManager.i.GetName(ID) + ": No items";
        return CharacterManager.i.GetName(ID) + ": " + string.Join(", ", Inventory.Select(x => x.Name));
    }

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
        var invString = Inventory.Count > 0 ? string.Join(",", Inventory.Select(item => item.ID)) : "";
        var resultString = ID + "~" + _happiness.ToString() + "~" + invString;
        SaveSystem.SaveDynamicData(resultString);

        //Debug.Log("saved dynamic data. happiness: " + _happiness);
    }
    private void SaveToFile() => SaveToFile(ID);

    /// <summary>
    /// Given an ID, load character's dynamic data from file.
    /// </summary>
    public void LoadFromFile(ID ID)
    {
        var saveStrings = SaveSystem.ReadFromFile(SaveSystem.dynamicDataFileName).Split('\n');
        foreach (var  s in saveStrings) {
            if (s.Split('~')[0] == ID) _ = LoadFromString(s);
        }

        this.ID = ID;
    }

    /// <summary>
    /// After loading the string for this character from the file,
    /// this method parses the string and loads the data into this instance.
    /// eventually will load in inventory, preferences, and other dynamic data.
    /// relationships are handled via characterManager.
    /// </summary>
    private async Task LoadFromString(string saveString)
    {
        await Task.Delay(100);

        var parts = saveString.Split("~");
        if (parts.Length < 2) return;

        _happiness = float.Parse(parts[1]);
        //Debug.Log("Loaded happiness for " + CharacterManager.i.GetNameFormatted(ID) + ": " + _happiness);
        if (parts.Length < 3) return;

        var itemIDs = parts[2].Split(',');
        foreach (var itemID in itemIDs) {
            if (itemID.Length < 2) continue;
            var item = TownGameManager.i.GetItemByID(itemID);
            if (item != null) AddToInventory(item);
        }
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

    public void IncreaseHappiness(float newHappiness) {
        _happiness= Mathf.Clamp(_happiness + newHappiness, 0f, 100f);
        SaveToFile();
    }
}

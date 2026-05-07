using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteCharacterData
{
    [HideInInspector] public string DisplayName;
    [SerializeField] private StaticCharacterData _staticData;
    [SerializeField] private DynamicCharacterData _dynamicData;

    private PersonalityData _personality;

    /// <summary>
    /// Loads character data for the character with the given static saveString from the save file.
    /// loads dynamic data based on ID
    /// </summary>
    public CompleteCharacterData(string staticSaveString)
    {
        _staticData = new StaticCharacterData();
        _staticData.FromStaticSaveString(staticSaveString);
        _personality = CharacterManager.i.GetPersonality(_staticData.ID);
        //Debug.Log(_staticData.Name + " personality: " + _personality.Type);

        _dynamicData = new DynamicCharacterData(_staticData.ID);
        DisplayName = _staticData.Name;
    }

    /// <summary>
    /// Given static character data, creates a complete character data instance with dynamic data initialized.
    /// used when spawning a character with a characterMeataController (i.e. in character creator or when loading a character into the world).
    /// </summary>
    /// <param name="staticData"></param>
    public CompleteCharacterData(StaticCharacterData staticData)
    {
        _staticData = staticData;
        _dynamicData = new DynamicCharacterData(_staticData.ID);
        DisplayName = _staticData.Name;
    }

    public ID ID => _staticData.ID;
    public string Name => _staticData.Name;
    public Gender Gender => _staticData.Gender;
    public Pronoun Pronouns => _staticData.Pronouns;
    public Attraction Attraction => _staticData.Attraction;
    public DateTime Birthday => _staticData.Birthday;
    public FavoriteColor FavColor => _staticData.FavColor;
    public int Age => SaveSystem.GetAge(_staticData.Birthday);
    public Sprite Icon => _staticData.Icon;

    //Happiness
    public float Happiness => _dynamicData.Happiness;
    public void SetHappiness(float newHappiness) {
        _dynamicData.IncreaseHappiness(-100);
        _dynamicData.IncreaseHappiness(newHappiness);
    }
    public void IncreaseHappiness(float newHappiness) => _dynamicData.IncreaseHappiness(newHappiness);

    //Inventory
    public List<ItemData> Inventory => _dynamicData.Inventory;
    public void AddToInventory(ItemData item) => _dynamicData.AddToInventory(item);
    public string GetInventoryString() => _dynamicData.GetInventoryString();

    //Problems
    /// <summary>
    /// Solves the problem but doesn't actually remove it because the rewards haven't been dispensed yet.
    /// that happens when the character is spoken to - this structure is so that minigame problems are marked complete when the
    /// minigame is finished, then rewards are given when the player is back in the characters room talking to them.
    /// </summary>
    public void SolveProblem() => _dynamicData.SolveProblem();
    public void GiveProblemRewards() => _dynamicData.GiveProblemRewards();
    public void SetProblem(ProblemData problem)
    {
        _dynamicData.CurrentProblem = GameObject.Instantiate(problem);
        _dynamicData.CurrentProblem.IsSolved = false;
    }

    public bool HasProblem => _dynamicData.CurrentProblem != null;
    public ProblemData CurrentProblem => _dynamicData.CurrentProblem;


    public string GetDialogue()
    {
        if (_dynamicData.CurrentProblem == null) return _personality.GetRandomLine();
        else return _dynamicData.CurrentProblem.Dialogue();
    }

}

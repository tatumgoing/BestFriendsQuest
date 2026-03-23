using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteCharacterData
{
    [HideInInspector] public string DisplayName;
    [SerializeField] private StaticCharacterData _staticData;
    [SerializeField] private CharacterData _dynamicData;

    /// <summary>
    /// Loads character data for the character with the given static saveString from the save file.
    /// loads dynamic data based on ID
    /// </summary>
    public CompleteCharacterData(string staticSaveString)
    {
        _staticData = new StaticCharacterData();
        _staticData.FromStaticSaveString(staticSaveString);

        _dynamicData = new CharacterData(_staticData.ID);
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
        _dynamicData = new CharacterData(_staticData.ID);
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

    //House
    public CharacterRoomModel RoomScript => _dynamicData.RoomScript;
    public void SetRoomScript(CharacterRoomModel newHouse) => _dynamicData.RoomScript = newHouse; 

    //Happiness
    public float Happiness => _dynamicData.Happiness;
    public void SetHappiness(float newHappiness) => _dynamicData.Happiness = newHappiness;
    public void IncreaseHappiness(float newHappiness)
    {
        _dynamicData.Happiness += newHappiness;
        _dynamicData.Happiness = Mathf.Clamp(_dynamicData.Happiness, 0, 100);
    }
    
    //Problems
    public void SolveProblem() => _dynamicData.SolveProblem();
    public void SetProblem(Problem problem) => _dynamicData.CurrentProblem = problem;
    public bool HasProblem => _dynamicData.CurrentProblem != null;
    public Problem CurrentProblem => _dynamicData.CurrentProblem;

}

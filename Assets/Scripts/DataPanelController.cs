using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FavoriteColor {RED, ORANGE, YELLOW, KIWI, FOREST, NAVY, BLUE, PINK, PURPLE, BROWN, WHITE, BLACK}
public enum ProfileDataType {NAME, GENDER, PRONOUN, ATTRACTION, DAY, MONTH, YEAR, COLOR}
public enum Gender { MALE, FEMALE, NONBINARY}
public enum Pronoun { HE, SHE, THEY}
[System.Flags]
public enum Attraction
{
    NONE = 0,
    MALE = 1 << 0,
    FEMALE = 1 << 1,
    NONBINARY = 1 << 2
}

[System.Serializable]
public class CharacterProfileData
{
    public string Name;
    public Gender Gender;
    public Pronoun Pronouns;
    public Attraction Attraction;
    public DateTime Birthday;
    public FavoriteColor FavColor;

    public CharacterProfileData()
    {
        Birthday = new DateTime();
    }
}

public class DataPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _gender;
    private CharacterProfileData _currentData = new CharacterProfileData();

    private void Start()
    {
        _currentData = new CharacterProfileData();
    }

    public void UpdateAttraction(Attraction gender, bool state)
    {
        var current = _currentData.Attraction;
        if (state) current |= gender;
        else current &= ~gender;
        _currentData.Attraction = current;

        print("updated attraction data. attracted to " + gender + ": " + state);
    }

    public void SetData(ProfileDataType type, string value)
    {
        print("recieved value for category: " + type + ", value: " + value);

        if (type == ProfileDataType.NAME) _currentData.Name = value;
        if (type == ProfileDataType.GENDER) _currentData.Gender = Enum.Parse<Gender>(value);
        if (type == ProfileDataType.PRONOUN) _currentData.Pronouns = Enum.Parse<Pronoun>(value);
        if (type == ProfileDataType.ATTRACTION) _currentData.Attraction = Enum.Parse<Attraction>(value);
        if (type == ProfileDataType.COLOR) _currentData.FavColor = Enum.Parse<FavoriteColor>(value);

        var birthday = _currentData.Birthday;
        var day = birthday.Day;
        var month = birthday.Month;
        var year = birthday.Year;

        if (type == ProfileDataType.DAY) day = int.Parse(value);
        if (type == ProfileDataType.MONTH) month = int.Parse(value);
        if (type == ProfileDataType.YEAR) year = int.Parse(value);

        _currentData.Birthday = new DateTime(year, month, day);
    }
}

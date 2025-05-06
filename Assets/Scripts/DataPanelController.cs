using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    private const string seperator = "%";
    private const string dateTimeFormat = "MMddyyyy";

    public CharacterProfileData()
    {
        Birthday = new DateTime();
    }

    public override string ToString()
    {
        var list = new List<string>
        {
            Name,
            Utils.EnumInt(Gender),
            Utils.EnumInt(Pronouns),
            Utils.EnumInt(Attraction),
            Birthday.ToString(dateTimeFormat),
            Utils.EnumInt(FavColor)
        };

        return string.Join(seperator, list);
    }

    public void FromString(string inputString)
    {
        Debug.Log("input string: " + inputString);
        var parts = inputString.Split(seperator);
        Name = parts[0];
        Gender = Utils.IntEnum<Gender>(parts[1]);
        Pronouns = Utils.IntEnum<Pronoun>(parts[2]);
        Attraction = Utils.IntEnum<Attraction>(parts[3]);
        Birthday = DateTime.ParseExact(parts[4], dateTimeFormat, CultureInfo.InvariantCulture);
        FavColor = Utils.IntEnum<FavoriteColor>(parts[5]);
    }

}

public class DataPanelController : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;

    private CharacterProfileData _currentData = new CharacterProfileData();

    private void Awake()
    {
        _currentData = new CharacterProfileData();
    }

    public void LoadFromString(string inputString)
    {
        _currentData.FromString(inputString);
        _characterController.Data = _currentData;
    }

    public void UpdateAttraction(Attraction gender, bool state)
    {
        var current = _currentData.Attraction;
        if (state) current |= gender;
        else current &= ~gender;
        _currentData.Attraction = current;

        //print("updated attraction data. attracted to " + gender + ": " + state);
    }

    public void SetName(string name) => SetData(ProfileDataType.NAME, name.Replace("%", ""));

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
        _characterController.Data = _currentData;
    }

    [ButtonMethod]
    private void PrintCurrent()
    {
        print(_currentData);
    }
}

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

[System.Flags, System.Serializable]
public enum Attraction
{
    NONE = 0,
    MALE = 1 << 0,
    FEMALE = 1 << 1,
    NONBINARY = 1 << 2
}

public class DataPanelController : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TMP_Dropdown _gender;
    [SerializeField] private TMP_Dropdown _pronoun;
    [SerializeField] private List<CheckBox> _attractionOptions = new List<CheckBox>();
    [SerializeField] private List<TMP_Dropdown> _birthdayDropdowns = new List<TMP_Dropdown>();
    [SerializeField] private TMP_Dropdown _colorDropdown;
    [SerializeField] private Animator _mainPanel;

    private StaticCharacterData _currentData = new StaticCharacterData();

    private void Awake()
    {
        //_currentData = new CharacterProfileData();
    }

    private void OnEnable()
    {
        _mainPanel.SetTrigger("Right");
    }

    public void Load(StaticCharacterData inputData)
    {
        _currentData = inputData;

        _nameField.text = _currentData.Name;

        _gender.SetValueWithoutNotify((int)_currentData.Gender);
        _pronoun.SetValueWithoutNotify((int)_currentData.Pronouns);

        _attractionOptions[0].SetCheckedVisual((_currentData.Attraction & Attraction.MALE) != 0);
        _attractionOptions[1].SetCheckedVisual((_currentData.Attraction & Attraction.FEMALE) != 0);
        _attractionOptions[2].SetCheckedVisual((_currentData.Attraction & Attraction.NONBINARY) != 0);

        _birthdayDropdowns[0].SetValueWithoutNotify(_currentData.Birthday.Month - 1);
        _birthdayDropdowns[1].SetValueWithoutNotify(_currentData.Birthday.Day - 1);
        _birthdayDropdowns[2].SetValueWithoutNotify(2025 - _currentData.Birthday.Year);

        _colorDropdown.value = (int)_currentData.FavColor;
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
        //print("recieved value for category: " + type + ", value: " + value);

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

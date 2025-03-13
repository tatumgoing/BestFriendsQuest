using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DropdownDataType { ENUM, RANGE, MONTH, COLOR}
public enum ProfileDataEnum { GENDER, PRONOUN, ATTRACTION}

[System.Serializable] public class ListWrapper<T> { public List<T> List = new List<T>();  }
[System.Serializable] public class ColorData { public FavoriteColor Color; public Sprite Sprite; }

public class SetDropdownOptions : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    [Space()]
    [SerializeField] private DropdownDataType _type;

    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.ENUM)] private ProfileDataEnum _enum;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.RANGE)] private Vector2Int _range;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.MONTH)] private TMP_Dropdown _monthDropdown;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.MONTH)] private ListWrapper<int> _monthDayCounts = new ListWrapper<int>();
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.COLOR)] private ListWrapper<ColorData> _colors = new ListWrapper<ColorData>();

    [Space()]
    [SerializeField] private ProfileDataType _sendType;

    private List<string> _current = new List<string>();
    private DataPanelController _controller;

    private void Start()
    {
        if (_type == DropdownDataType.RANGE) AddRange(_range, true);
        if (_type == DropdownDataType.MONTH) {
            SetDays(0);
            _monthDropdown.onValueChanged.AddListener(SetDays);
        }
        if (_type == DropdownDataType.ENUM) {
            var list = new List<string>();
            if (_enum == ProfileDataEnum.GENDER) list = Utils.EnumToList<Gender>().Select(x => x.ToString()).ToList();
            if (_enum == ProfileDataEnum.PRONOUN) list = Utils.EnumToList<Pronoun>().Select(x => x.ToString()).ToList();
            if (_enum == ProfileDataEnum.ATTRACTION) list = Utils.EnumToList<Attraction>().Select(x => x.ToString()).ToList();
            SetOptions(list);
        }
        if (_type == DropdownDataType.COLOR) {
            var options = new List<TMP_Dropdown.OptionData>();
            _current.Clear();
            foreach (var c in _colors.List) {
                options.Add(new TMP_Dropdown.OptionData(c.Color.ToString(), c.Sprite));
                _current.Add(c.Color.ToString());
            }
            _dropdown.options = options;
        }

        _dropdown.onValueChanged.AddListener(UpdateData); 
        _controller = GetComponentInParent<DataPanelController>();
    }

    private void AddRange(Vector2Int range, bool descending = false)
    {
        var optionList = new List<string>();
        for (int i = range.x; i <= range.y; i++) {
            optionList.Add(i.ToString());
        }
        if (descending) optionList.Reverse();
        SetOptions(optionList);
    }

    private void SetOptions(List<string> input)
    {
        var optionsList = new List<TMP_Dropdown.OptionData>();
        foreach (var opt in input) optionsList.Add(new TMP_Dropdown.OptionData(opt));
        _dropdown.options = optionsList;
        _current = input;
    }

    private void SetDays(int monthIndex)
    {
        AddRange(new Vector2Int(1, _monthDayCounts.List[monthIndex]));
    }

    private void UpdateData(int index)
    {
        _controller.SetData(_sendType, _current[index]);
    }
}

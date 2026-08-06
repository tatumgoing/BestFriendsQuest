using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DropdownDataType { ENUM, RANGE, DAY_MONTH, COLOR, MONTH_ABR, YEAR}
public enum ProfileDataEnum { GENDER, PRONOUN, ATTRACTION}
public enum MonthAbrev { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec}

[System.Serializable] public class ListWrapper<T> { public List<T> List = new List<T>();  }

[System.Serializable] 
public class ColorData {
    [HideInInspector] public string DisplayName;
    public FavoriteColor Color; 
    public Sprite Sprite;
    public Color UseColor;
}

public class SetDropdownOptions : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    [Space()]
    [SerializeField] private DropdownDataType _type;

    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.ENUM)] private ProfileDataEnum _enum;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.RANGE)] private Vector2Int _range;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.DAY_MONTH)] private TMP_Dropdown _monthDropdown;
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.DAY_MONTH)] private ListWrapper<int> _monthDayCounts = new ListWrapper<int>();
    [SerializeField, ConditionalField(nameof(_type), false, false, DropdownDataType.COLOR)] private ListWrapper<ColorData> _colors = new ListWrapper<ColorData>();

    [Space()]
    [SerializeField] private ProfileDataType _sendType;

    private List<string> _current = new List<string>();
    private DataPanelController _controller;


    private void Start()
    {
        if (!_dropdown) return;

        if (_type == DropdownDataType.RANGE) AddRange(_range, true);
        if (_type == DropdownDataType.YEAR) {
            var currentYear = System.DateTime.Now.Year;
            AddRange(new Vector2Int(currentYear-100, currentYear), true);
        }
        if (_type == DropdownDataType.DAY_MONTH) {
            SetDays(0);
            _monthDropdown.onValueChanged.AddListener(SetDays);
        }
        if (_type == DropdownDataType.MONTH_ABR) {
            var abrs = Utils.EnumToList<MonthAbrev>().Select(x => x.ToString()).ToList();
            SetOptions(abrs);
        }
        if (_type == DropdownDataType.ENUM) {
            var list = new List<string>();
            if (_enum == ProfileDataEnum.GENDER) list = Utils.EnumToList<Gender>().Select(x => x.ToString()).ToList();
            if (_enum == ProfileDataEnum.PRONOUN) list = new List<string>() { "HE/HIM", "THEY/THEM", "SHE/HER" };
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

        UpdateData(0);
    }

    public Color GetColor(int selection) => _colors.List[selection].UseColor;
    public Color GetColor(FavoriteColor color)
    {
        var data = _colors.List.Where(x => x.Color == color).FirstOrDefault();
        if (data == default) {
            Debug.LogError("Color not found in dropdown colors list: " + color.ToString());
        }
        return data.UseColor;
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
        if (!_controller) return;
        var selectedValue = _current[index];
        if (_type == DropdownDataType.MONTH_ABR) selectedValue = (index + 1).ToString();
        if (_type == DropdownDataType.ENUM && _enum == ProfileDataEnum.PRONOUN) selectedValue = ((Pronoun) index).ToString();

        _controller.SetData(_sendType, selectedValue);
    }
}

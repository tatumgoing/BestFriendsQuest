using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FavoriteColorClothingInterface : MonoBehaviour
{
    [SerializeField] private ListWrapper<ColorData> _colors = new ListWrapper<ColorData>();
    [SerializeField] private SetDropdownOptions _optionsScript;
    [SerializeField] private SetMaterialField _shirt;

    public void SetColor(FavoriteColor color)
    {
        if (_optionsScript != null) {
            var colorValue = _optionsScript.GetColor(color);
            _shirt.SetColor(colorValue);
        }
        else {
            var data = _colors.List.Where(x => x.Color == color).FirstOrDefault();
            _shirt.SetColor(data.UseColor);
        }
    }

    public void SetColor(int selection)
    {
        if (!_optionsScript) return;

        var color = _optionsScript.GetColor(selection);
        _shirt.SetColor(color);
    }
}

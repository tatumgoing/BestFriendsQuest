using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteColorClothingInterface : MonoBehaviour
{
    [SerializeField] private SetDropdownOptions _optionsScript;
    [SerializeField] private SetMaterialField _shirt;

    public void SetColor(FavoriteColor color)
    {
        var colorValue = _optionsScript.GetColor(color);
        _shirt.SetColor(colorValue);
    }

    public void SetColor(int selection)
    {
        var color = _optionsScript.GetColor(selection);
        _shirt.SetColor(color);
    }
}

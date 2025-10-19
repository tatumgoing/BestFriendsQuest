using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteColorClothingInterface : MonoBehaviour
{
    [SerializeField] private SetDropdownOptions _optionsScript;
    [SerializeField] private SetMaterialField _shirt;

    public void SetColor(int selection)
    {
        var color = _optionsScript.GetColor(selection);
        _shirt.SetColor(color);
    }
}

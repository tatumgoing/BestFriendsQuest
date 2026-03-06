using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(ColorMenuController))]
public class HairColorMenu : MonoBehaviour
{
    private ColorMenuController _controller;
    [SerializeField] private HairController _hairController;
    [SerializeField] private SelectableItem _matchCheckBox;

    private bool _matching;

    private void OnEnable()
    {
        if (!_controller) _controller = GetComponent<ColorMenuController>();
        _controller.SetFromHexCode(_hairController.Current.GetSettings().Color.ToHex());
        _matching = _hairController.Current.GetSettings().MatchColor;
        if (_matching) _matchCheckBox.Select(true, false);
        else _matchCheckBox.Deselect(true, false);
    }

    public void SetMatch(bool match)
    {
        _matching = match;
        _hairController.Current.As<HairPiece>().SetMatch(match);
        if (match) {
            _controller.SetFromHexCode(_hairController.HairColor.ToHex());
            _hairController.Current.SetColor(_hairController.HairColor);
        }
    }

    public void SetColor(Color color)
    {
        if (_hairController.Current.GetSettings().MatchColor) _hairController.SetHairColor(color);
        else _hairController.SetCurrentColor(color);
    }
}

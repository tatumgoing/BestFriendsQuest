using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicColorOption : MonoBehaviour
{
    [SerializeField] private SelectableItem _button;
    [SerializeField] private Image _color;
    [SerializeField] private Tooltip _toolTip;

    private ColorMenuController _controller;
    public Color Color => _color.color;

    public void SelectButton() => _button.Select();

    public void Initialize(BasicColorData data, ColorMenuController controller)
    {
        _controller = controller;
        _toolTip.UpdateText(data.Name);
        _color.color = data.Color;
    }

    public void Select()
    {
        _controller.SelectBasicColor(_color.color, this);
    }

    public void Deselect()
    {
        _button.Deselect(true, false);
    }
}

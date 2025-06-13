using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOption : MonoBehaviour
{
    [SerializeField] private Image _preview;
    private FeatureSOData _data;

    public FeatureSubType Type => _data.SubType;

    public FeatureSOData GetData() => _data;

    public void Initialize(FeatureSOData data)
    {
        _preview.sprite = data.Icon;
        _data = data;
    }

    public void EarSelect()
    {
        GetComponentInParent<EarShapeMenu>().SelectShape(_data);
    }

    public void Select()
    {
        GetComponentInParent<LayersMenuController>().AddFeature(_data);
    }
}

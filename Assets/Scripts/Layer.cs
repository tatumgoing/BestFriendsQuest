using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layer : MonoBehaviour
{
    private LayersMenuController _controller;
    [SerializeField] private Image _preview;
    private FeatureObj _feature;

    public FeatureObj GetFeature() => _feature;

    private void Start()
    {
        _controller = GetComponentInParent<LayersMenuController>();
    }

    public void Initialize(FeatureObj feature)
    {
        _feature = feature;
        _preview.sprite = _feature.GetData().Icon;
    }

    public void Select()
    {
        if (!_controller) _controller = GetComponentInParent<LayersMenuController>();
        _controller.Select(this, _feature);
    }

    public void Delete()
    {
        _controller.DeleteFeature(this, _feature);
    }

    public void Duplicate()
    {
        _controller.Duplicate(_feature);
    }
}

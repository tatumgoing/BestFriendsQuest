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
    [SerializeField] private RectTransform _arrowButtonTransform;
    [SerializeField] private Tooltip _arrowTooltip;

    private FeatureTier _tier;

    public FeatureObj GetFeature() => _feature;

    private void Start()
    {
        _controller = GetComponentInParent<LayersMenuController>();
    }

    public void Initialize(FeatureObj feature, FeatureTier tier)
    {
        _feature = feature;
        _preview.sprite = _feature.GetData().Icon;
        _tier = tier;
        if (tier == FeatureTier.BASE) {
            _arrowButtonTransform.localScale = new Vector3(-1, 1, 1);
            _arrowTooltip.UpdateText("Switch to Detail");
        }
    }

    public void Select()
    {
        if (!_controller) _controller = GetComponentInParent<LayersMenuController>(true);
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

    public void EnableMirror()
    {
        _controller.EnableMirror();
    }

    public void DisableMirror()
    {
        _controller.DisableMirror();
    }

    public void SwitchTier()
    {
        _controller.SwitchTier(this, _tier);

        if (_tier == FeatureTier.DETAIL) {
            _tier = FeatureTier.BASE;
            _arrowButtonTransform.localScale = new Vector3(-1, 1, 1);
            _arrowTooltip.UpdateText("Switch to Detail");
        }
        else {
            _tier = FeatureTier.DETAIL;
            _arrowButtonTransform.localScale = Vector3.one;
            _arrowTooltip.UpdateText("Switch to Base");
        }
    }
}

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
    [SerializeField] private GameObject _changeTierArrowParent;

    private FeatureTier _tier;

    public FeatureObj GetFeature() => _feature;

    private void Start()
    {
        _controller = GetComponentInParent<LayersMenuController>();
        //if (_changeTierArrowParent) _changeTierArrowParent.SetActive(_controller.ShowingDetails());
    }

    public void Initialize(FeatureObj feature, FeatureTier tier, LayersMenuController controller)
    {
        _feature = feature;
        _preview.sprite = _feature.GetData().Icon;
        _tier = tier;
        if (tier == FeatureTier.BASE) {
            _arrowButtonTransform.localScale = new Vector3(-1, 1, 1);
            _arrowTooltip.UpdateText("Switch to Detail");
        }

        gameObject.name = feature.GetData().name + " Layer";

        if (!controller.ShowingDetails()) {
            if (_tier == FeatureTier.BASE) _feature.GetComponent<FacialFeature>().SetInBack();
            else _feature.GetComponent<FacialFeature>().SetOnTop();
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
        _controller.EnableMirror(_feature);
    }

    public void DisableMirror()
    {
        _controller.DisableMirror(_feature);
    }

    public void SwitchTier()
    {
        _controller.SwitchTier(this, _tier);

        if (_tier == FeatureTier.DETAIL) {
            _tier = FeatureTier.BASE;

            _arrowButtonTransform.localScale = new Vector3(-1, 1, 1);
            if (_controller.ShowingDetails()) _arrowTooltip.UpdateText("Switch to Detail");
            else _feature.GetComponent<FacialFeature>().SetInBack();
        }
        else {
            _tier = FeatureTier.DETAIL;
            _arrowButtonTransform.localScale = Vector3.one;
            if (_controller.ShowingDetails()) _arrowTooltip.UpdateText("Switch to Base");
            else _feature.GetComponent<FacialFeature>().SetOnTop();
        }
    }
}

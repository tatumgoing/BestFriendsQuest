using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveMenuController : MonoBehaviour
{
    [SerializeField, OverrideLabel("feature controller")] private GameObject _controllerObj;
    private IFeatureController _controller;
    [SerializeField] private Slider _horiSlider;
    [SerializeField] private Slider _vertSlider;
    [SerializeField] private Slider _sizeSlider;
    [SerializeField] private Slider _angleSlider;
    [SerializeField] private List<SelectableItem> _mirrorOptions = new List<SelectableItem>();
    private FeatureObj _current;

    [SerializeField] private UnityEvent _onChangePosition;

    private void OnEnable()
    {
        _controller ??= _controllerObj.GetComponent<IFeatureController>(); 
        _current = _controller.GetCurrent();

        var currentValues = _current.GetSettings();
        _horiSlider.value = currentValues.Hori;
        _vertSlider.value = currentValues.Vert;
        _sizeSlider.value = currentValues.Size;
        _angleSlider.value = currentValues.Angle;

        _mirrorOptions[(int)_current.GetSettings().Mirror].Select();
    }

    public void SetVert() => _current.SetVert(_vertSlider.value);
    public void SetHori() => _current.SetHori(_horiSlider.value);
    public void SetSize() => _current.SetSize(_sizeSlider.value);
    public void SetAngle() => _current.SetAngle(_angleSlider.value);
    public void SetMirror(int type) => _current.SetMirrorTpe((MirrorType) type);
}

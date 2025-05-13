using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    [SerializeField] private GameObject _checkMark;
    [SerializeField] private Image _checkboxImg;
    [SerializeField] private bool _checked = false;

    [ConditionalField(nameof(_manual), true)] public UnityEvent<bool> OnChange;
    [SerializeField] private bool _manual;
    [SerializeField, ConditionalField(nameof(_manual))] private UnityEvent _onToggleOn;
    [SerializeField, ConditionalField(nameof(_manual))] private UnityEvent _onToggleOff;

    private void OnValidate()
    {
        _checkMark.SetActive(_checked);
    }

    private void Start()
    {
        _checkMark.SetActive(_checked);
    }

    public void SetCheckedVisual(bool isChecked)
    {
        _checked = isChecked;
        _checkMark.SetActive(isChecked);
    }

    public void Toggle()
    {
        if (_checked) ToggleOff();
        else ToggleOn();
        if (!_manual) OnChange.Invoke(_checked);
    }

    public void ToggleOn()
    {
        _checked = true;
        if (_manual) _onToggleOn.Invoke();
        _checkMark.SetActive(true);
    }

    public void ToggleOff()
    {
        _checked = false;
        if (_manual) _onToggleOff.Invoke();
        _checkMark.SetActive(false);
    }

}

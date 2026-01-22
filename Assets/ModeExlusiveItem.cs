using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ModeExlusiveItem : MonoBehaviour
{
    [SerializeField] private bool _setActiveState = true;
    [SerializeField, ConditionalField(nameof(_setActiveState))] private GameMode _activeMode = GameMode.ADVANCED;
    [SerializeField] private UnityEvent<GameMode> _onChangeMode;
    [SerializeField] private bool _setText;
    [SerializeField, ConditionalField(nameof(_setText))] private TextMeshProUGUI _text;
    [SerializeField, ConditionalField(nameof(_setText))] private string _advancedText;
    [SerializeField, ConditionalField(nameof(_setText))] private string _simpleText;

    public void UpdateMode(GameMode mode)
    {
        if (_setActiveState) gameObject.SetActive(mode == _activeMode);
        _onChangeMode.Invoke(mode);
        if (_setText && _text) _text.text = mode == GameMode.ADVANCED ? _advancedText : _simpleText;
    }
}

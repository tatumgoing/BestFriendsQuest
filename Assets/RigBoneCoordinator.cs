using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigBoneCoordinator : MonoBehaviour
{
    [SerializeField] private BoneName _name;
    private Dictionary<BoneSliderName, BoneSettingsData> _data = new Dictionary<BoneSliderName, BoneSettingsData>();
    private Dictionary<BoneSliderName, float> _currentSettings = new Dictionary<BoneSliderName, float>();
    [SerializeField] private bool _disablePositionOffset;

    private Vector3 _startingLocalPosition;
    private bool _initialized = false;

    public BoneName Name => _name;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        _startingLocalPosition = transform.localPosition;
    }

    public void RegisterScaleData(BoneSettingsData scaleData, BoneSliderName category)
    {
        if (scaleData.AffectedBone != _name) return;

        if (_data.ContainsKey(category)) {
            _data[category] = scaleData;
        }
        else _data.Add(category, scaleData);     
        
        //print("registered " + category + " settings of " + scaleData.PositionOffset + " for " + _name + " bone.");
    }

    public void UpdateValue(BoneSliderName category, float value)
    {
        //print("tring to update " + category + " settings of " + value + " for " + _name + " bone. contains: " + _data.ContainsKey(category));
        if (!_data.ContainsKey(category)) return;

        _data[category].UpdateValue(value);        
    }

    public void UpdatePositionAndScale()
    {
        if (_initialized) Initialize();
        if (!_disablePositionOffset) UpdatePosition();
        UpdateScale();
    }

    private void UpdatePosition()
    {
        Vector3 targetPosition = _startingLocalPosition;
        foreach (var data in _data.Values) {
            //print("Data: " + data.PositionOffset);
            targetPosition += data.PositionOffset;
        }
        transform.localPosition = targetPosition;
    }

    private void UpdateScale()
    {
        float targetScale = 1;
        foreach (var data in _data.Values) {
            targetScale *= data.ScaleMod;
        }
        transform.localScale = targetScale * Vector3.one;
    }
}

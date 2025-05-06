using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarController : MonoBehaviour, IFeatureController
{
    [SerializeField] private Ear _currentEar;
    [SerializeField] private List<FeatureData> _allOptions = new List<FeatureData>();

    public bool HasCurrent() => true;
    public FeatureObj GetCurrent() => _currentEar;

    private void Start()
    {
        _currentEar = GetComponentInChildren<Ear>();
        _currentEar.initilize();
    }

    public void LoadFromString(string saveString)
    {
        var parts = saveString.Split("~");
        FeatureData selected = null;
        foreach (var f in _allOptions) if (f.Icon.name == parts[0]) selected = f;
        _currentEar.SetData(selected);
        _currentEar.ConfigureFromString(parts[1]);
    }

    public string GetSaveString()
    {
        return _currentEar.ToString();
    }

    public void UpdateCurrentEar(FeatureData data)
    {
        _currentEar.SetData(data);
    }

    public void SetColor(Color skinColor)
    {
        _currentEar.SetColor(skinColor);
    }

    public FeatureObj AddFeature(FeatureData data)
    {
        _currentEar.As<Ear>().SetData(data);
        return _currentEar;
    }

    public List<FeatureData> GetAllOptions()
    {
        return _allOptions;
    }

    public List<FeatureObj> GetCurrentFeatures()
    {
        var list = new List<FeatureObj>();
        list.Add(_currentEar);
        return list;
    }

    public void Save(FeatureData data)
    {
        for (int i = 0; i < _allOptions.Count; i++) {
            if (_allOptions[i].EarPrefab == data.EarPrefab) {
                _allOptions[i] = data;
                return;
            }
        }
        _allOptions.Add(data);
    }

    //unused interface methods:
    public void Delete(FeatureObj feature) { }
    public void CopySettingsToCurrent(FeatureObj original) { }
    public void Select(FeatureObj feature) { }

}

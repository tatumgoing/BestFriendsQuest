using MyBox;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FaceFeatureController : MonoBehaviour, IFeatureController
{
    [SerializeField] private GameObject _featurePrefab;
    [SerializeField] private Transform _featureParent;
    public List<FacialFeature> CurrentFeatures = new List<FacialFeature>();
    [SerializeField] private List<FeatureData> _allFeatures = new List<FeatureData>();
    [SerializeField] private int _selected;

    private FeatureCategory _currentCategory;

    public bool HasCurrent() => CurrentFeatures.Count > 0;
    public FacialFeature Current => _selected < CurrentFeatures.Count ? CurrentFeatures[_selected] : CurrentFeatures[0];
    public List<FeatureObj> GetCurrentFeatures() => CurrentFeatures.Cast<FeatureObj>().Where(x => x.GetData().Category == _currentCategory).ToList();
    public List<FeatureData> GetAllOptions() => _allFeatures;
    public void CopySettingsToCurrent(FeatureObj original) => original.CopyTo(Current);
    public FeatureObj GetCurrent() => Current;

    private void OnValidate()
    {
        foreach (var f in _allFeatures) f.Name = f.Texture.name;
    }

    private void Start()
    { 
        CurrentFeatures = GetComponentsInChildren<FacialFeature>().Where(x => !x.IsMirroredVersion).ToList();
    }

    public void LoadFromString(string saveString)
    {
        var toDelete = new List<FacialFeature>(CurrentFeatures);
        foreach (var f in toDelete) Delete(f);
        if (string.IsNullOrWhiteSpace(saveString)) return;

        var parts = saveString.Split("&");
        foreach (var p in parts) {
            AddFeatureFromString(p);
        }
    }

    private void AddFeatureFromString(string featureString)
    {
        var parts = featureString.Split("~");
        FeatureData selected = null;
        foreach (var f in _allFeatures) if (f.Icon.name == parts[0]) selected = f;
        var newFeature = AddFeature(selected);
        newFeature.ConfigureFromString(parts[1]);
    }

    public string GetSaveString()
    {
        var list = new List<string>();
        foreach (var f in CurrentFeatures) list.Add(f.ToString());
        return string.Join("&", list);
    }

    public void SetCurrentColor(Color color)
    {
        Current.SetColor(color);
    }

    public FeatureObj AddFeature(FeatureData data)
    {
        data.Category = _currentCategory;
        var newFeature = Instantiate(_featurePrefab, _featureParent).GetComponent<FacialFeature>();
        newFeature.transform.SetAsFirstSibling();
        newFeature.Set(new FeatureData(data));
        CurrentFeatures.Add(newFeature);
        return newFeature;
    }

    public void Delete(FeatureObj featureGeneric)
    {
        var feature = (FacialFeature) featureGeneric;
        CurrentFeatures.Remove(feature);
        Destroy(feature.gameObject);
    }

    public void Select(FeatureObj featureGeneric)
    {
        var feature = (FacialFeature) featureGeneric;
        for (int i = 0; i < CurrentFeatures.Count; i++) {
            if (CurrentFeatures[i] == feature) _selected = i;
        }
    }

    public void SaveFeature(FeatureData data)
    {
        data.Name = data.Texture.name;

        bool found = false;
        for (int i = 0; i < _allFeatures.Count; i++) {
            if (_allFeatures[i].Texture == data.Texture) {
                _allFeatures[i] = data;
                found = true;
            }
        }
        if (!found) _allFeatures.Add(data);
        Utils.SetDirty(this);
    }

    public void SetCategory(FeatureCategory category)
    {
        _currentCategory = category;
    }
}

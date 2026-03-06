using MyBox;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FaceFeatureController : MonoBehaviour, IFeatureController
{
    [SerializeField] private GameObject _featurePrefab;
    [SerializeField] private Transform _featureParent;
    [SerializeField] private int _selected;
    [SerializeField] private bool _inCharacterCreator;

    [HideInInspector] public List<FacialFeature> CurrentFeatures = new List<FacialFeature>();

    [ReadOnly] private List<FeatureSOData> _allFeatures = new List<FeatureSOData>();
    private FeatureCategory _currentCategory;
    private FeatureTier _currentPriority;

    public bool HasCurrent() => GetCurrentFeatures().Count > 0;
    public FacialFeature Current => _selected < CurrentFeatures.Count ? CurrentFeatures[_selected] : CurrentFeatures[0];
    public List<FeatureObj> GetCurrentFeatures() => CurrentFeatures.Where(x => x.Category == _currentCategory).Cast<FeatureObj>().ToList();
    public List<FeatureSOData> GetAllOptions() => _allFeatures;
    public void CopySettingsToCurrent(FeatureObj original) => original.CopyTo(Current);
    public FeatureObj GetCurrent() => Current;
    public void SetCategory(FeatureCategory category) => _currentCategory = category;
    public void SetPriority(FeatureTier priority) => _currentPriority = priority;

    private void OnValidate()
    {
        foreach (var f in _allFeatures) if (f != null) f.Name = f.Texture.name;
    }

    private void Awake()
    {
        if (_allFeatures.Count == 0) Initialize();
    }

    public void Reset()
    {
        foreach (var feature in CurrentFeatures) if (feature) Destroy(feature.gameObject);
        CurrentFeatures = new List<FacialFeature>();
    }

    private void Initialize()
    {
        _allFeatures = Resources.LoadAll<FeatureSOData>("FacialFeatures").OrderByDescending(x => x.Priority).ToList();
    }

    private void Start()
    { 
        CurrentFeatures = GetComponentsInChildren<FacialFeature>().Where(x => !x.IsMirroredVersion).ToList();
    }

    public void SetExpression(ExpressionData expression, ExpressionData secondary = null, ExpressionData Tertiary = null)
    {
        if (expression == null) return;

        ResetExpression();
        foreach (var category in expression.Data) SetExpressionForCategory(category);
        if (secondary) foreach (var category in secondary.Data) SetExpressionForCategory(category);
        if (Tertiary) foreach (var category in Tertiary.Data) SetExpressionForCategory(category);
    }

    private void ResetExpression()
    {
        foreach (var feature in CurrentFeatures) if (feature) feature.SetExpression(new ExpressionPieceData());
    }

    private void SetExpressionForCategory(ExpressionPieceData data)
    {
        var affectedFeatures = CurrentFeatures.Where(x => x.Category == data.Category);
        foreach (var f in affectedFeatures) f.SetExpression(data);
    }

    public void LoadFromString(string saveString)
    {
        var toDelete = new List<FacialFeature>(CurrentFeatures);
        foreach (var f in toDelete) Delete(f);
        if (string.IsNullOrWhiteSpace(saveString)) return;

        var parts = saveString.Split("&");
        foreach (var p in parts) {
            AddFeatureFromString(p, _inCharacterCreator);
        }
    }

    private void AddFeatureFromString(string featureString, bool inCharacterCreator = false)
    {
        if (_allFeatures.Count == 0) Initialize();

        var parts = featureString.Split("~");
        FeatureSOData selected = null;
        foreach (var f in _allFeatures) if (f.Icon.name == parts[0]) selected = f;

        var newFeature = AddFeature(selected);
        newFeature.ConfigureFromString(parts[1]);
        newFeature.As<FacialFeature>().SetScaleMode(inCharacterCreator);
        
    }

    public string GetSaveString()
    {
        var list = new List<string>();
        foreach (var f in CurrentFeatures) list.Add(f.ToString());
        return string.Join("&", list);
    }

    public void SetCurrentColor(Color color)
    {
        //print("current: " + Current.name + " set to color: " + color.ToHex());
        Current.SetColor(color);
    }

    public FeatureObj AddFeature(FeatureSOData data)
    {
        //print("trying to add feature. data == null: " + (data == null));
        var newFeature = Instantiate(_featurePrefab, _featureParent).GetComponent<FacialFeature>();
        newFeature.transform.SetAsFirstSibling();
        newFeature.Set(data, _currentCategory, _currentPriority);
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

    public void SaveFeature(FeatureSOData data)
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

}

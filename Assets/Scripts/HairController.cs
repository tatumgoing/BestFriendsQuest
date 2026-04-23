using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HairController : MonoBehaviour, IFeatureController
{
    [SerializeField] private GameObject _featurePrefab;
    [SerializeField] private Transform _featureListParent;
    [SerializeField] private List<HairPiece> _currentPieces = new List<HairPiece>();
    [SerializeField] private int _currentIndex;
    [SerializeField] private Vector2 _limits;
    [SerializeField] private Vector2 _scaleLimits = new Vector2(0.2f, 1.8f);
    [SerializeField] private Transform _target;
    [SerializeField] private ColorMenuController _color;

    private List<FeatureSOData> _allOptions = new List<FeatureSOData>();

    public Color HairColor { get; private set; }
    public bool HasCurrent() => _currentPieces.Count > 0;
    public FeatureObj Current => _currentPieces[_currentIndex];
    public FeatureObj GetCurrent() => Current;
    public List<FeatureSOData> GetAllOptions() => _allOptions;
    public List<FeatureObj> GetCurrentFeatures() => _currentPieces.Cast<FeatureObj>().ToList();

    private void Awake()
    {
        if (_allOptions.Count == 0) Initialize();
    }

    private void Initialize()
    {
        _allOptions = Resources.LoadAll<FeatureSOData>("HairFeatures").OrderByDescending(x => x.Priority).ToList();
    }

    private void Start()
    {
        if (UIManager.i != null) UIManager.i.OnTabSwitch.AddListener(DeselectAllAddons);
        
        if (_color) HairColor = _color.GetDefaultColor();
        //else HairColor = Color.red;

        _currentPieces = GetComponentsInChildren<HairPiece>().Where(x => !x.IsMirroredVersion).ToList();
        foreach (var c in _currentPieces) c.Initialize(this);
        foreach (var c in _currentPieces) if (c.GetSettings().MatchColor || c.GetData().IsMainHair) c.SetColor(HairColor);
    }

    private void DeselectAllAddons()
    {
        if (!FindObjectOfType<AddonsUIHelper>().Addons) return;

        foreach (var i in _currentPieces) {
            var addon = i.GetComponentInChildren<MovableAddon>();
            if (addon) addon.SetSelected(false);
        }
    }

    public void LoadFromString(string saveString)
    {
        var toDelete = new List<HairPiece>(_currentPieces);
        foreach (var f in toDelete) Delete(f);
        if (string.IsNullOrWhiteSpace(saveString)) return;

        var parts = saveString.Split("&");
        foreach (var p in parts) {
            //print("adding hair feature: " + p);
            AddFeatureFromString(p);
        }
    }

    private void AddFeatureFromString(string featureString)
    {
        if (_allOptions.Count == 0) Initialize();

        var parts = featureString.Split("~");
        FeatureSOData selected = null;
        foreach (var f in _allOptions) if (f.name == parts[0]) selected = f;

        //foreach (var f in _allOptions) if (f.Icon.name != parts[0]) print("|" + f.Icon.name + " != " + parts[0]);
        //if (selected == null) print("couln't find " + parts[0] + " feature. count: " + _allFeatures.Count);

        var newFeature = AddFeature(selected);
        newFeature.ConfigureFromString(parts[1]);

        if (newFeature.GetSettings().MatchColor || _currentPieces.Count == 1) {
            HairColor = newFeature.GetSettings().Color;
        }

        // print("Adding hair feature from string. name: " + selected.name + ", featureString: " + featureString + ", number part: " + parts[1]);
    }

    public string GetSaveString()
    {
        var list = new List<string>();
        foreach (var f in _currentPieces) list.Add(f.ToString());
        return string.Join("&", list);
    }

    public void SetCurrentColor(Color newColor)
    {
        //print("setting current color: " + newColor.ToHex());

        if (Current.GetData().IsMainHair) SetHairColor(newColor);
        Current.SetColor(newColor);
    }

    public void SetHairColor(Color newColor)
    {
        if (string.Equals(HairColor.ToHex(), newColor.ToHex())) return;

        //print("setting HAIR COLOR: " + newColor.ToHex() + ", previous: " + HairColor.ToHex());

        foreach (var h in _currentPieces) {
            if (h.GetSettings().MatchColor || h.GetData().IsMainHair) h.SetColor(newColor);
        }
        HairColor = newColor;
    }

    public Vector3 GetTargetPosition(float hori, float vert)
    {
        var pos = _target.localPosition;
        var x = Mathf.Lerp(-_limits.x, _limits.x, hori);
        var z = Mathf.Lerp(-_limits.y, _limits.y, vert);
        pos.x = x;
        pos.z = z;
        _target.localPosition = pos;
        return _target.position;
    }

    public void SetSize(float size)
    {
        _currentPieces[_currentIndex].transform.GetChild(0).GetChild(0).transform.localScale = Vector3.one * Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, size);
    }

    public void SetAngle(float angle)
    {
        _currentPieces[_currentIndex].transform.GetChild(0).GetChild(0).localEulerAngles = Vector3.up * Mathf.Lerp(-180, 180, angle);
    }

    public void CopySettingsToCurrent(FeatureObj original)
    {
        original.CopyTo(Current);
    }

    public FeatureObj AddFeature(FeatureSOData data)
    {
        //print("adding hair. isMain: " + data.IsMainHair + ", name: " +  data.name);
        if (data.IsMainHair) {
            for (int i = _currentPieces.Count - 1; i >= 0; i--) {
                if (_currentPieces[i].GetData().IsMainHair) {
                    print("Deleting main hair: " + _currentPieces[i].GetData().name);
                    Delete(_currentPieces[i]);
                }
            }
        }

        var newFeature = Instantiate(_featurePrefab, _featureListParent).GetComponent<HairPiece>();
        //Debug.LogError("Instantiating hair piece: " + data.name);
        //print("initializing new hair piece: " + data.name);

        newFeature.Initialize(data, this);
        _currentPieces.Add(newFeature);
        _currentIndex = _currentPieces.Count - 1;
        if (newFeature.GetSettings().MatchColor) newFeature.SetColor(HairColor);
        newFeature.SetAll(newFeature.GetDefaults());

        if (!data.IsMainHair) {
            var originalMirrorType = newFeature.GetDefaults().Mirror;
            newFeature.SetMirrorTpe(MirrorType.BOTH);
            newFeature.SetMirrorTpe(originalMirrorType);
        }

        return newFeature;
    }

    public void Delete(FeatureObj feature)
    {
        //print("Deleting hair feature: " + feature.GetData().name);
        if (Current == feature) _currentIndex = Mathf.Max(0, _currentIndex - 1);
        _currentPieces.Remove((HairPiece)feature);
        Destroy(feature.gameObject);
    }

    public void Select(FeatureObj feature)
    {
        for (int i = 0; i < _currentPieces.Count; i++) {
            if (feature == _currentPieces[i]) {
                _currentIndex = i;
                _currentPieces[i].SetSelected(true);
                //print("Selected: " + _currentIndex + ", Name: " + feature.GetData().Name + ", name: " + feature.GetData().name);
            }
            else _currentPieces[i].SetSelected(false);
        }
    }

    public void Save(FeatureSOData data)
    {
        for (int i = 0; i < _allOptions.Count; i++) {
            if (data.EarPrefab == _allOptions[i].EarPrefab) {
                _allOptions[i] = data;
                return;
            }
        }
        _allOptions.Add(data);
        Utils.SetDirty(this);
    }

    public void SetCategory(FeatureCategory category)
    {
        throw new System.NotImplementedException();
    }
}

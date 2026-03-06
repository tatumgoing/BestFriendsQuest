using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum FeatureTier { BASE, DETAIL};

public class LayersMenuController : MonoBehaviour
{
    [SerializeField] private List<SelectableItem> _otherTabButtons = new List<SelectableItem>();
    [SerializeField, OverrideLabel("Feature Controller")] private GameObject _featureControllerMB;
    [SerializeField] private GameObject _main;
    [SerializeField] private AddMenuController _addMenu;
    [SerializeField] private GameObject _layerPrefab;
    [SerializeField] private Transform _baseLayerListParent;
    [SerializeField] private Transform _detailLayerListParent;
    [SerializeField] private Transform _aboveLayerListParent;
    [SerializeField] private FaceMenu _faceMenu;
    [SerializeField] private ColorMenuController _colorMenu;

    [Header("Options")]
    [SerializeField] private GameObject _detailsParent;
    [SerializeField] private TextMeshProUGUI _mainListHeader;
    [SerializeField] private GameObject _topParent;
    [SerializeField] private bool _hairAddonLayers;
    [SerializeField] private bool _transitionToMoveAutomatically;
    [SerializeField, ConditionalField(nameof(_transitionToMoveAutomatically))] private SelectableItem _moveMenuButton;
    [SerializeField] private Slider _addonSizeSlider;

    [SerializeField] private UnityEvent OnAddFeature;

    private List<Layer> _spawnedLayers = new List<Layer>();
    private FeatureTier _currentTier;
    private FeatureSubType _currentSubType;
    private IFeatureController _featureController;

    public bool ShowingDetails() {
        if (_detailsParent) return _detailsParent.activeInHierarchy;
        return false;
    }

    public int NumLayers => _spawnedLayers.Count;

    public FeatureObj GetCurrent() => _featureController.GetCurrent();
    public void OpenAddMenuBase() => OpenAddMenu(FeatureTier.BASE);
    public void OpenAddMenuDetails() => OpenAddMenu(FeatureTier.DETAIL);

    private void OnEnable()
    {
        _main.SetActive(true);
        _addMenu.gameObject.SetActive(false);
        SelectInitial();

        if (_featureController == null) Initialize(_currentSubType);
    }

    private void Start()
    {
        if (_hairAddonLayers) Initialize(FeatureSubType.ADDONS);
    }

    public void SetShowDetails(bool showDetails = true)
    {
        _detailsParent.SetActive(showDetails);
        _topParent.SetActive(!showDetails);
        _mainListHeader.text = showDetails ? "Base" : "Below";
    }

    public void SetScale(float scale)
    {
        _featureController.GetCurrent().SetSize(scale);
    }

    public bool HasCurrent()
    {
        if (_featureController == null) Initialize();
        return _featureController.HasCurrent();
    }

    private void OpenAddMenu(FeatureTier tier)
    {
        if (_faceMenu) {
            _featureController.As<FaceFeatureController>().SetPriority(tier);
        }
        _currentTier = tier;
        if (tier == FeatureTier.BASE) {
            _addMenu.GetComponent<AddMenuController>().ChangeCategory(_currentSubType);
        }
        else {
            _addMenu.GetComponent<AddMenuController>().ChangeCategory(FeatureSubType.MISC);
        }
        _addMenu.gameObject.SetActive(true);
    }

    public void SwitchTier(Layer layer, FeatureTier oldTier)
    {
        var oldSiblingIndex = layer.transform.GetSiblingIndex();

        var newTier = oldTier == FeatureTier.BASE ? FeatureTier.DETAIL : FeatureTier.BASE;

        if (_faceMenu) {
            layer.GetFeature().As<FacialFeature>().SetTier(newTier);
        }

        var newParent = oldTier == FeatureTier.BASE ? _detailLayerListParent : _baseLayerListParent;
        if (!ShowingDetails() && newParent == _detailLayerListParent) newParent = _aboveLayerListParent; 
        layer.transform.SetParent(newParent);

        if (newParent.transform.childCount > oldSiblingIndex) layer.transform.SetSiblingIndex(oldSiblingIndex);
    }

    public void EnableMirror(FeatureObj feature)
    {
        feature.SetMirrorTpe(MirrorType.BOTH);
    }

    public void DisableMirror(FeatureObj feature)
    {
        feature.SetMirrorTpe(MirrorType.LEFT);
    }

    public void Duplicate(FeatureObj original)
    {
        _currentTier = original.Tier;
        //print("duplicated a " +  original.Tier);

        AddFeature(original.GetData(), true);
        _featureController.CopySettingsToCurrent(original);
    }

    public void AddFeature(FeatureSOData data, bool duplicate = false)
    {
        if (_featureController == null) Initialize();

        var added = _featureController.AddFeature(data);
        AddLayer(added);
        _spawnedLayers[^1].GetComponent<SelectableItem>().Select();

        if (!duplicate) {
            added.SetDefaults();
        }

        if (_addMenu.gameObject.activeInHierarchy) {
            _main.SetActive(true);
            _addMenu.gameObject.SetActive(false);
        }

        if (_colorMenu) {
            _colorMenu.SetColor(data.DefaultSettings.Color);
            added.SetColor(data.DefaultSettings.Color);
        }

        if (_transitionToMoveAutomatically) {
            _moveMenuButton.Select();
        }

        if (_hairAddonLayers && data.DefaultSettings.Mirror != MirrorType.BOTH) {
            EnableMirror(added);
            DisableMirror(added);
        }

        OnAddFeature.Invoke();
    }

    public void Initialize(FeatureSubType category = FeatureSubType.ALL)
    {
        _currentSubType = category;
        _featureController = _featureControllerMB.GetComponent<IFeatureController>();

        BuildLayerList();
        _addMenu.BuildAddList(_featureController);
        UpdateTabButtons();
        SelectInitial();
    }

    private void SelectInitial()
    {
        if (_featureController == null) {
            Initialize(_currentSubType);
            if (_featureController == null) return;
        }

        if (_hairAddonLayers && _spawnedLayers.Count > 0) {
            _spawnedLayers[0].GetComponent<Layer>().Select();
        }
        else {

            for (int i = _spawnedLayers.Count - 1; i >= 0; i--) {
                var layer = _spawnedLayers[i];

                if (layer.GetFeature().GetData().SubType == _currentSubType && layer.GetFeature() == GetCurrent()) {
                    _spawnedLayers[i].GetComponent<SelectableItem>().Select(true);
                    break;
                }
            }

            for (int i = _spawnedLayers.Count - 1; i >= 0; i--) {
                var layer = _spawnedLayers[i];

                if (layer.GetFeature().GetData().SubType == _currentSubType) {
                    _spawnedLayers[i].GetComponent<SelectableItem>().Select(true);
                    break;
                }
            }
        }
    }

    private void UpdateTabButtons()
    {
        foreach (var b in _otherTabButtons) {
            if (b != _otherTabButtons[0]) b.SetDisabled(!_featureController.HasCurrent());
        }

        _otherTabButtons[0].Select(true, false);
    }

    public void DeleteFeature(Layer layer, FeatureObj feature)
    {
        _spawnedLayers.Remove(layer);
        Destroy(layer.gameObject);
        _featureController.Delete(feature);
        UpdateTabButtons();

        if (_spawnedLayers.Count > 0) _spawnedLayers[^1].Select();
    }

    private void BuildLayerList()
    {
        foreach (var l in _spawnedLayers) Destroy(l.gameObject);
        _spawnedLayers.Clear();

        foreach (var feature in _featureController.GetCurrentFeatures()) {
            var facialFeature = feature.As<FacialFeature>();
            if (facialFeature) _currentTier = facialFeature.Tier;
            AddLayer(feature);
        }

        SelectInitial();
    }

    private void AddLayer(FeatureObj feature)
    {
        if (_hairAddonLayers && feature.GetData().SubType != FeatureSubType.ADDONS) return;

        var parent = _currentTier == FeatureTier.BASE ? _baseLayerListParent : _detailLayerListParent;
        if (!ShowingDetails() && parent == _detailLayerListParent) parent = _aboveLayerListParent;

        var newLayer = Instantiate(_layerPrefab, parent).GetComponent<Layer>();

        newLayer.transform.SetAsFirstSibling();
        newLayer.Initialize(feature, _currentTier, this);
        _spawnedLayers.Add(newLayer);
        UpdateTabButtons();
    }

    /// <summary>
    /// called when the layer is clicked 
    /// </summary>
    public void Select(Layer layerObj, FeatureObj feature)
    {
        foreach (var l in _spawnedLayers) {
            var button = l.GetComponent<SelectableItem>();
            if (button.GetComponent<Layer>() != layerObj) button.Deselect(true, false);
        }
        _featureController.Select(feature);

        if (_addonSizeSlider) _addonSizeSlider.value = feature.GetSettings().Size;

        if (_faceMenu) _faceMenu.SwitchSelectedLayer(layerObj);

        if (_colorMenu) _colorMenu.SetColor(feature.GetSettings().Color);
    }
}

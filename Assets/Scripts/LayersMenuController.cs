using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersMenuController : MonoBehaviour
{
    [SerializeField] private List<SelectableItem> _otherTabButtons = new List<SelectableItem>();
    [SerializeField, OverrideLabel("Feature Controller")] private GameObject _featureControllerMB;
    [SerializeField] private GameObject _main;
    [SerializeField] private AddMenuController _addMenu;
    [Header("Main")]
    [SerializeField] private GameObject _layerPrefab;
    [SerializeField] private Transform _layerListParent;
    public bool HasCurrent => _featureController.HasCurrent();

    private List<Layer> _spawnedLayers = new List<Layer>();

    private IFeatureController _featureController;
    
    private void OnEnable()
    {
        _main.SetActive(true);
        _addMenu.gameObject.SetActive(false);
        SelectInitial();
    }

    public FeatureObj GetCurrent() => _featureController.GetCurrent();

    public void Duplicate(FeatureObj original)
    {
        AddFeature(original.GetData());
        _featureController.CopySettingsToCurrent(original);
    }

    public void AddFeature(FeatureData data)
    {
        var added = _featureController.AddFeature(data);
        AddLayer(added);
        _spawnedLayers[^1].GetComponent<SelectableItem>().Select();

        if (_addMenu.gameObject.activeInHierarchy) {
            _main.SetActive(true);
            _addMenu.gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        _featureController = _featureControllerMB.GetComponent<IFeatureController>();
        BuildLayerList();
        _addMenu.BuildAddList(_featureController);
        UpdateTabButtons();
        SelectInitial();
    }

    private void SelectInitial()
    {
        if (_featureController == null) return;
        foreach (var layer in _spawnedLayers) {
            if (layer.GetFeature() == _featureController.GetCurrent()) layer.GetComponent<SelectableItem>().Select();
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
    }

    private void BuildLayerList()
    {
        foreach (var l in _spawnedLayers) Destroy(l.gameObject);
        _spawnedLayers.Clear();

        foreach (var feature in _featureController.GetCurrentFeatures()) AddLayer(feature);
    }

    private void AddLayer(FeatureObj feature)
    {
        var newLayer = Instantiate(_layerPrefab, _layerListParent).GetComponent<Layer>();
        newLayer.transform.SetAsFirstSibling();
        newLayer.Initialize(feature);
        _spawnedLayers.Add(newLayer);
        UpdateTabButtons();
    }

    public void Select(int siblingIndex, FeatureObj feature)
    {
        foreach (var l in _spawnedLayers) {
            var button = l.GetComponent<SelectableItem>();
            if (button.transform.GetSiblingIndex() != siblingIndex) button.Deselect(true, false);
        }
        _featureController.Select(feature);
    }
}

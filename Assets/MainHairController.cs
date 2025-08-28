using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainHairController : MonoBehaviour
{
    [SerializeField] private List<FeatureSOData> _hairData;
    [SerializeField] private Image _currentlySelectedImage;
    [SerializeField] private SelectableItem _previousButton;
    [SerializeField] private SelectableItem _nextButton;
    [SerializeField] private GameObject _optionPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private Scrollbar _listScrollbar;
    [SerializeField] private HairController _controller;

    private int _currentlySelectedIndex;
    private List<MainHairOption> _spawnedOptions = new List<MainHairOption>();

    private void Awake()
    {
        _hairData = Resources.LoadAll<FeatureSOData>("HairFeatures").ToList();
    }

    private void OnEnable()
    {
        if (_hairData == null || _currentlySelectedIndex >= _hairData.Count) return;
        UpdateVisuals();
        BuildList();
    }

    private void BuildList()
    {
        foreach (var spawned in _spawnedOptions) if (spawned) Destroy(spawned.gameObject);
        _spawnedOptions.Clear();
        foreach (var data in _hairData) SpawnOption(data);
    }

    private void SpawnOption(FeatureSOData hairData)
    {
        var newOption = Instantiate(_optionPrefab, _listParent).GetComponent<MainHairOption>();
        newOption.transform.SetSiblingIndex(_listParent.transform.childCount - 2);
        newOption.Initialize(hairData, this);

        _spawnedOptions.Add(newOption);
    }

    public void SelectNext()
    {
        if (_currentlySelectedIndex < _hairData.Count - 1) _currentlySelectedIndex += 1;
        UpdateVisuals();
    }

    public void SelectPrevious()
    {
        if (_currentlySelectedIndex > 0) _currentlySelectedIndex -= 1;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _controller.AddFeature(_hairData[_currentlySelectedIndex]);

        _listScrollbar.value = (float) _currentlySelectedIndex / (_hairData.Count - 1);
        _currentlySelectedImage.sprite = _hairData[_currentlySelectedIndex].Icon;
        _previousButton.SetDisabled(_currentlySelectedIndex == 0);
        _nextButton.SetDisabled(_currentlySelectedIndex >= _hairData.Count-1);
    }

    public void Select(FeatureSOData data, MainHairOption selectedOption)
    {
        foreach (var option in _spawnedOptions) if (option && option != selectedOption) option.GetComponent<SelectableItem>().Deselect();

        for (int i = 0; i < _hairData.Count; i++) {
            if (_hairData[i] == data) _currentlySelectedIndex = i;
        }
        UpdateVisuals();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHairController : MonoBehaviour
{
    [SerializeField] private List<FeatureSOData> _hairData;
    [SerializeField] private GameObject _optionPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private HairController _controller;
    [SerializeField] private FeatureSOData _defaultHair;
    [SerializeField] private ColorMenuController _color;

    [Header("Page Buttons")]
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private SelectableItem _previousButton;
    [SerializeField] private SelectableItem _nextButton;

    private int _currentlySelectedIndex;
    private List<MainHairOption> _spawnedOptions = new List<MainHairOption>();
    private bool _initialized;

    private int _currentPage = 0;

    private void Awake()
    {
        if (!_initialized) Initialize();
    }

    private void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        _hairData = Resources.LoadAll<FeatureSOData>("HairFeatures").Where(x => x.IsMainHair).ToList();
        for (int i = 0; i < _hairData.Count; i++) {
            if (_hairData[i] == _defaultHair) _currentlySelectedIndex = i;
        }
    }

    private void OnEnable()
    {
        if (_hairData == null || _currentlySelectedIndex >= _hairData.Count) return;
        UpdateVisuals();
        BuildList();
    }

    private void BuildList()
    {
        _previousButton.SetDisabled(_currentPage == 0);

        foreach (var spawned in _spawnedOptions) if (spawned) Destroy(spawned.gameObject);
        _spawnedOptions.Clear();

        for (int i = 0; i < _hairData.Count; i++) {
            if (i < (_currentPage +1) * 9 && i+1 > (_currentPage) * 9) {
                SpawnOption(_hairData[i]);
            }
        }

        _pageText.text = (_currentPage + 1) + "/" + (Mathf.CeilToInt(_hairData.Count / 9) + 1);
    }

    public void NextPage()
    {
        if ((_currentPage + 1)* 9 >= _hairData.Count) return;

        _currentPage += 1;
        BuildList();

        _previousButton.SetDisabled(false);
        if ((_currentPage + 1) * 9 >= _hairData.Count) _nextButton.SetDisabled(true);
    }

    public void PreviousPage()
    {
        if (_currentPage == 0) return;

        _currentPage -= 1;
        BuildList();

        _nextButton.SetDisabled(false);
        if (_currentPage == 0) _previousButton.SetDisabled(true);
    }

    private void SpawnOption(FeatureSOData hairData)
    {
        var newOption = Instantiate(_optionPrefab, _listParent).GetComponent<MainHairOption>();
        newOption.transform.SetSiblingIndex(_listParent.transform.childCount - 2);
        newOption.Initialize(hairData, this);

        if (_spawnedOptions.Count == _currentlySelectedIndex) {
            newOption.GetComponent<SelectableItem>().Select(true, false);
        }

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

    public void UpdateVisuals()
    {
        if (!_initialized) Initialize();

        _controller.AddFeature(_hairData[_currentlySelectedIndex]);
        _controller.SetCurrentColor(_color.GetColor());

        foreach (var option in _spawnedOptions) {
            if (option && option.Feature != _hairData[_currentlySelectedIndex]) option.GetComponent<SelectableItem>().Deselect(false);
            else option.GetComponent<SelectableItem>().Select(true, false);
        }
    }

    public void Select(FeatureSOData data, MainHairOption selectedOption)
    {
        for (int i = 0; i < _hairData.Count; i++) {
            if (data == _hairData[_currentlySelectedIndex]) return;
        }

        foreach (var option in _spawnedOptions) if (option && option != selectedOption) option.GetComponent<SelectableItem>().Deselect();

        for (int i = 0; i < _hairData.Count; i++) {
            if (_hairData[i] == data) _currentlySelectedIndex = i;
        }
        UpdateVisuals();
    }
}

using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pageNumText;
    [SerializeField] private Transform _categoryParent;
    [SerializeField] private Scrollbar _slider;
    [SerializeField] private bool _usePages;
    [SerializeField, ConditionalField(nameof(_usePages))] private int _numPerPage = 20;
    [SerializeField] private SelectableItem _previousButton;
    [SerializeField] private SelectableItem _nextButton;

    [Header("Add")]
    [SerializeField] private GameObject _addPrefab;
    [SerializeField] private Transform _addListParent;

    private List<AddOption> _spawnedAddOptions = new List<AddOption>();
    private int _numPages = 0;
    private int _currentPage = 0;
    private FeatureSubType _currentDisplayType = FeatureSubType.MISC;


    private void OnEnable()
    {
        _currentPage = 0;
        _slider.value = 0;
        _slider.onValueChanged.Invoke(0);
        ChangeCategory(_currentDisplayType);
    }

    public void BuildAddList(IFeatureController faceController)
    {
        foreach (var a in _spawnedAddOptions) Destroy(a.gameObject);
        _spawnedAddOptions.Clear();

        foreach (var o in faceController.GetAllOptions()) AddOption(o);
    }

    private void AddOption(FeatureSOData feature)
    {
        var newAddOption = Instantiate(_addPrefab, _addListParent).GetComponent<AddOption>();
        newAddOption.Initialize(feature);
        _spawnedAddOptions.Add(newAddOption);
    }

    public void NextPage()
    {
        if (_currentPage < _numPages-1) _currentPage += 1;
        ChangeCategory(_currentDisplayType);
    }

    public void PreviousPage()
    {
        if (_currentPage > 0) _currentPage -= 1;
        ChangeCategory(_currentDisplayType);
    }

    public void UpdatePageButtons()
    {
        _previousButton.SetDisabled(_currentPage <= 0);
        _nextButton.SetDisabled(_currentPage >= _numPages - 1);
    }

    public void DeselectCategories() => ChangeCategory(FeatureSubType.ALL);

    public void ChangeCategory(FeatureSubType type)
    {

        _currentDisplayType = type;

        foreach (var button in _categoryParent.GetComponentsInChildren<AddMenuCategoryButton>()) {
            if (type != button.Type) button.GetComponent<SelectableItem>().Deselect(true, false);
        }

        var validOptions = new List<GameObject>();
        foreach (var option in _spawnedAddOptions) {
            var valid = type == FeatureSubType.ALL || option.Type == type;
            if (_usePages) {
                option.gameObject.SetActive(false);
                if (valid) validOptions.Add(option.gameObject);
            }
            else option.gameObject.SetActive(valid);
        }

        if (_usePages) {
            _numPages = Mathf.CeilToInt((validOptions.Count -1) / _numPerPage) + 1;
            _pageNumText.text = (_currentPage + 1) + "/" + _numPages; 

            for (int i = _currentPage * _numPerPage; i < Mathf.Min((_currentPage+1) * _numPerPage, validOptions.Count); i++) {
                validOptions[i].SetActive(true);
            }
            UpdatePageButtons();
        }

        //Category = type;
        //BuildAddList();
    }
}

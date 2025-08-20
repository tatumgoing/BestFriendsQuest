using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHairController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _hairSprites;
    [SerializeField] private Image _currentlySelectedImage;
    [SerializeField] private SelectableItem _previousButton;
    [SerializeField] private SelectableItem _nextButton;
    [SerializeField] private GameObject _optionPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private Scrollbar _listScrollbar;

    private int _currentlySelectedIndex;
    private List<MainHairOption> _spawnedOptions = new List<MainHairOption>();

    private void OnEnable()
    {
        UpdateVisuals();
        BuildList();
    }

    private void BuildList()
    {
        foreach (var spawned in _spawnedOptions) Destroy(spawned.gameObject);
        foreach (var sprite in _hairSprites) SpawnOption(sprite);
    }

    private void SpawnOption(Sprite sprite)
    {
        var newOption = Instantiate(_optionPrefab, _listParent).GetComponent<MainHairOption>();
        newOption.transform.SetSiblingIndex(_listParent.transform.childCount - 2);
        newOption.Initialize(sprite, this);

        _spawnedOptions.Add(newOption);
    }

    public void SelectNext()
    {
        if (_currentlySelectedIndex < _hairSprites.Count - 1) _currentlySelectedIndex += 1;
        UpdateVisuals();
    }

    public void SelectPrevious()
    {
        if (_currentlySelectedIndex > 0) _currentlySelectedIndex -= 1;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _listScrollbar.value = (float) _currentlySelectedIndex / (_hairSprites.Count - 1);
        _currentlySelectedImage.sprite = _hairSprites[_currentlySelectedIndex];
        _previousButton.SetDisabled(_currentlySelectedIndex == 0);
        _nextButton.SetDisabled(_currentlySelectedIndex >= _hairSprites.Count-1);
    }

    public void Select(Sprite sprite, MainHairOption selectedOption)
    {
        foreach (var option in _spawnedOptions) if (option && option != selectedOption) option.GetComponent<SelectableItem>().Deselect();

        for (int i = 0; i < _hairSprites.Count; i++) {
            if (_hairSprites[i] == sprite) _currentlySelectedIndex = i;
        }
        UpdateVisuals();
    }
}

using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemListDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _listItemPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private bool _pageMode;
    [SerializeField, ConditionalField(nameof(_pageMode))] private int _numPerPage; 
    [SerializeField, ConditionalField(nameof(_pageMode))] private SelectableItem _prevButton; 
    [SerializeField, ConditionalField(nameof(_pageMode))] private SelectableItem _nextButton; 
    [SerializeField, ConditionalField(nameof(_pageMode))] private TextMeshProUGUI _pageDisplayText; 

    [Header("Headers and footers")]
    [SerializeField] private bool _hasHeader;
    [SerializeField, ConditionalField(nameof(_hasHeader))] private Transform _header;
    [SerializeField] private bool _hasFooter;
    [SerializeField, ConditionalField(nameof(_hasFooter))] private Transform _footer;

    private int _currentPage;   
    private List<IListIItem> _spawnedItems = new List<IListIItem>();
    private Func<ItemData, bool> _currentFilter = (x => x);

    public void ShowSelected(Func<ItemData, bool> predicate)
    {
        if (predicate != _currentFilter) _currentPage = 0; 

        foreach (var item in _spawnedItems) {
            if (predicate(item.Item)) item.Show();
            else item.Hide();
        }
        _currentFilter = predicate;

        if (_pageMode) UpdatePageDisplay();
    }

    public void DisplayItems(List<ItemData> items, IItemListController controller)
    {
        foreach (var item in _spawnedItems) item?.Destroy();
        _spawnedItems.Clear();

        foreach (var item in items) SpawnItem(item, controller);

        if (_spawnedItems.Count > 0) _spawnedItems[0].SetSelected();

        if (_pageMode) UpdatePageDisplay();
    }

    public void PrevPage()
    {
        _currentPage -= 1;
        UpdatePageDisplay();
    }

    public void NextPage()
    {
        _currentPage += 1;
        UpdatePageDisplay();
    }

    private void UpdatePageDisplay()
    {
        for (int i = 0; i < _spawnedItems.Count; i++) _spawnedItems[i].SetActive(false);

        var validItems = _spawnedItems.Where(x => _currentFilter(x.Item)).ToList();
        for (int i = 0; i < validItems.Count; i++) {
            bool shouldBeActive = i >= _currentPage * _numPerPage && i < (_currentPage + 1) * _numPerPage;
            validItems[i].SetActive(shouldBeActive);
        }

        var maxPages = Mathf.FloorToInt(validItems.Count() / (float)_numPerPage);
        _prevButton.SetDisabled(_currentPage == 0);
        _nextButton.SetDisabled(_currentPage >= maxPages);
        _pageDisplayText.text = (_currentPage + 1) + "/" + (maxPages + 1);
    }

    private void SpawnItem(ItemData item, IItemListController controller)
    {
        var spawnedItem = Instantiate(_listItemPrefab, _listParent).GetComponent<IListIItem>();
        spawnedItem.Initialize(item, controller);
        _spawnedItems.Add(spawnedItem);

        if (_hasHeader) _header.SetAsFirstSibling();
        if (_hasFooter) _footer.SetAsLastSibling();
    }

    public void SetFirstSelected()
    {
        foreach (var item in _spawnedItems) {
            if (item.Active) {
                item.SetSelected();
                return;
            }
        }
    }

    public void DeselectNonMatching(ItemData item)
    {
        foreach (var i in _spawnedItems) {
            if (i.Item != item) i.Deselect();
        }
    }
}

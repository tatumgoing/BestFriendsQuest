using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemListDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _listItemPrefab;
    [SerializeField] private Transform _listParent;

    [Header("Headers and footers")]
    [SerializeField] private bool _hasHeader;
    [SerializeField, ConditionalField(nameof(_hasHeader))] private Transform _header;
    [SerializeField] private bool _hasFooter;
    [SerializeField, ConditionalField(nameof(_hasFooter))] private Transform _footer;

    private List<IListIItem> _spawnedItems = new List<IListIItem>();

    public void DisplayItem(List<ItemData> items, IItemListController controller)
    {
        foreach (var item in _spawnedItems) item?.Destroy();
        _spawnedItems.Clear();

        foreach (var item in items) SpawnItem(item, controller);

        if (_spawnedItems.Count > 0) _spawnedItems[0].SetSelected();
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
        if (_spawnedItems.Count > 0) _spawnedItems[0].SetSelected();
    }

    public void DeselectNonMatching(ItemData item)
    {
        foreach (var i in _spawnedItems) {
            if (i.Item != item) i.Deselect();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IItemListController
{
    [SerializeField] private ItemListDisplay _itemListDisplay;
    [SerializeField] private CurrentlySelectedItem _currentlySelectedItem;

    public void ShowClothing() => ChangeCategory(ItemType.Clothing);
    public void ShowFood() => ChangeCategory(ItemType.Food);
    public void ShowHousing() => ChangeCategory(ItemType.Housing);

    public void SelectItem(ItemData item)
    {
        _currentlySelectedItem.ShowItem(item);
        _itemListDisplay.DeselectNonMatching(item);
    }

    private void OnEnable()
    {
        var inventory = TownGameManager.i.GetInventoryItems();
        _itemListDisplay.DisplayItem(inventory, this);
    }

    private void ChangeCategory(ItemType type)
    {
        _itemListDisplay.ShowSelected(x => x.Type == type);
        _itemListDisplay.SetFirstSelected();
    }
}

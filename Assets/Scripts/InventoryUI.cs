using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IItemListController
{
    [SerializeField] private ItemListDisplay _itemListDisplay;
    [SerializeField] private CurrentlySelectedItem _currentlySelectedItem;
    [SerializeField] private GameObject _currentlySelectedParent;

    public void ShowClothing() => ChangeCategory(ItemType.Clothing);
    public void ShowFood() => ChangeCategory(ItemType.Food);
    public void ShowHousing() => ChangeCategory(ItemType.Housing);

    public void SelectItem(ItemData item)
    {
        _currentlySelectedItem.ShowItem(item);
        _itemListDisplay.DeselectNonMatching(item);
        _currentlySelectedParent.SetActive(true); 
    }

    private void OnEnable()
    {
        _currentlySelectedParent.SetActive(false);

        var inventory = TownGameManager.i.GetInventoryItems().OrderByDescending(x => TownGameManager.i.GetNumberOwned(x)).ToList();
        _itemListDisplay.DisplayItems(inventory, this);
    }

    private void ChangeCategory(ItemType type)
    {
        _currentlySelectedParent.SetActive(false);
        _itemListDisplay.ShowSelected(x => x.Type == type);
        _itemListDisplay.SetFirstSelected();
    }
}

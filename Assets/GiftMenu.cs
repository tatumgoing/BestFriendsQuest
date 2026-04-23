using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GiftMenu : MonoBehaviour, IItemListController
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private ItemListDisplay _itemListDisplay;
    [SerializeField] private CurrentlySelectedItem _currentlySelectedItem;

    private ItemData _currentItem;
    private ID _id;

    public void ShowClothing() => ChangeCategory(ItemType.Clothing);
    public void ShowFood() => ChangeCategory(ItemType.Food);
    public void ShowHousing() => ChangeCategory(ItemType.Housing);

    public void Show(ID id)
    {
        _id = id;
        gameObject.SetActive(true);
        _title.text = "A gift for " + CharacterManager.i.GetName(id) + "?";

        var items = TownGameManager.i.GetInventoryItems();
        _itemListDisplay.DisplayItem(items, this);

        ShowClothing();
    }

    void IItemListController.SelectItem(ItemData item)
    {
        _currentItem = item;
        _itemListDisplay.DeselectNonMatching(item);
        _currentlySelectedItem.ShowItem(item);
    }

    public void GiveGift()
    {
        TownGameManager.i.SubtractInventory(_currentItem);
        gameObject.SetActive(false);
        CharacterManager.i.GiveItem(_id, _currentItem);
    }

    private void ChangeCategory(ItemType type)
    {
        _itemListDisplay.ShowSelected(x => x.Type == type);
        _itemListDisplay.SetFirstSelected();    
    }
}

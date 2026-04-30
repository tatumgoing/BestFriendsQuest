using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public interface IItemListController
{
    public abstract void SelectItem(ItemData item);
}

public class ShopUIController : MonoBehaviour, IItemListController
{
    [SerializeField] private ItemType _type;
    [SerializeField] private ItemListDisplay _itemList;
    [SerializeField] private CurrentlySelectedItem _currentlySelected;
    [SerializeField] private SelectableItem _purchaseButton;
    [SerializeField] private ClothingShopController _areaController;

    // sorry Aidan

    [SerializeField] private Image _hoveringItem;

    [Header("Sounds")]
    [SerializeField] private Sound _purchaseSound;

    private ItemData _currentlySelectedItem;

    void Start()
    {
        _purchaseSound = Instantiate(_purchaseSound);
    }

    private void OnEnable()
    {
        if (!TownGameManager.i) {
            gameObject.SetActive(false);
            return;
        }
        BuildList();
        UpdatePurchaseButton();

        if (_hoveringItem) _hoveringItem.sprite = _currentlySelectedItem.sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)) {
            TownGameManager.i.ChangeCurrency(113);
            UpdatePurchaseButton();
        }
    }

    private void BuildList()
    {
        var selectedItems = TownGameManager.i.GetAllItems(true).Where(x => x.Type == _type).ToList();
        _itemList.DisplayItem(selectedItems, this);
        _itemList.SetFirstSelected();
    }

    public void BuyCurrent()
    {
        _purchaseSound.Play();
        TownGameManager.i.BuyItem(_currentlySelectedItem);
        UpdatePurchaseButton();
    }

    public void SelectItem(ItemData item)
    {
        _itemList.DeselectNonMatching(item);

        _currentlySelected.ShowItem(item);
        _currentlySelectedItem = item;

        _purchaseButton.gameObject.SetActive(true);
        UpdatePurchaseButton();

        _areaController?.DisplayItem(item);

        if (_hoveringItem) _hoveringItem.sprite = item.sprite;
    }

    private void UpdatePurchaseButton()
    {
        _purchaseButton.SetDisabled(TownGameManager.i.Currency < _currentlySelectedItem.Cost);
    }
}

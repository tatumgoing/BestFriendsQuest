using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private SelectableItem _purchaseButton;
    [SerializeField] private ItemType _type;
    [SerializeField] private GameObject _listItemPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private CurrentlySelectedShopItem _currentlySelected;

    [SerializeField] private ClothingShopController _areaController;

    [Header("Sounds")]
    [SerializeField] private Sound _purchaseSound;

    private List<ShopListItem> _spawnedItems = new List<ShopListItem>();
    private ItemData _currentlySelectedItem;


    void Start()
    {
        _purchaseSound = Instantiate(_purchaseSound);
    }

    private void OnEnable()
    {
        BuildList();
        UpdatePurchaseButton();
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
        foreach (var item in _spawnedItems) Destroy(item.gameObject);
        _spawnedItems.Clear();

        foreach (var item in TownGameManager.i.GetAllItems().Where(x => x.Type == _type)) SpawnItem(item);

        if (_spawnedItems.Count > 0) _spawnedItems[0].GetComponent<SelectableItem>().Select(true);
    }

    public void BuyCurrent()
    {
        _purchaseSound.Play();
        TownGameManager.i.BuyItem(_currentlySelectedItem);
        UpdatePurchaseButton();
    }

    private void SpawnItem(ItemData item)
    {
        var spawnedItem = Instantiate(_listItemPrefab, _listParent).GetComponent<ShopListItem>();
        spawnedItem.Initialize(item, this);
        spawnedItem.transform.SetSiblingIndex(_listParent.childCount - 2);
        _spawnedItems.Add(spawnedItem);
    }

    public void SelectItem(ItemData item)
    {
        foreach (var i in _spawnedItems) if (i.Item != item) i.Deselect();
        _currentlySelected.Initialize(item);
        _currentlySelectedItem = item;

        _purchaseButton.gameObject.SetActive(true);
        UpdatePurchaseButton();

        _areaController.DisplayItem(item);
    }

    private void UpdatePurchaseButton()
    {
        _purchaseButton.SetDisabled(TownGameManager.i.currency < _currentlySelectedItem.Cost);
    }
}

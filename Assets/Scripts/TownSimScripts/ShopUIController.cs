using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private GameObject _listItemPrefab;
    [SerializeField] private Transform _listParent;

    [SerializeField] private ClothingShopController _areaController;

    private List<ShopListItem> _spawnedItems = new List<ShopListItem>();




    [Header("Sidebar")]

    public GameObject descriptionContainer;

    public TMP_Text descriptionTextbox;
    public TMP_Text currentHeldTextbox;

    public GameObject purchaseButton;

    public Image spriteDisplay;

    [Header("Selection Menu")]
    public RecordsManager recordsManager;
    public List<ItemTabs> tabs = new List<ItemTabs>();

    void Start()
    {
        BuildList();
    }

    private void BuildList()
    {
        foreach (var item in _spawnedItems) Destroy(item.gameObject);
        _spawnedItems.Clear();

        foreach (var item in TownGameManager.i.GetAllItems()) SpawnItem(item);
    }

    private void SpawnItem(ItemData item)
    {
        var spawnedItem = Instantiate(_listItemPrefab, _listParent).GetComponent<ShopListItem>();
        spawnedItem.Initialize(item, this);
        _spawnedItems.Add(spawnedItem);
    }

    public void SelectItem(ItemData item)
    {
        foreach (var i in _spawnedItems) if (i.Item != item) i.Deselect();

        _areaController.DisplayItem(item);
    }

    void UpdateTab(ItemTabs clickedTab)
    {
        foreach(ItemTabs tab in tabs)
        {
            if (tab != clickedTab)
            {
                tab.selected = false;
            }
        }

        clickedTab.selected = true;
    }

    public void UpdatePurchasedButton()
    {
        purchaseButton.GetComponent<BuyItem>().item = recordsManager.selectedBanner.itemID;
        purchaseButton.GetComponent<BuyItem>().Puchased();
    }
}

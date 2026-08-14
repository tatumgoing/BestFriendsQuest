using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CategoryGameObjectLink
{
    public ItemType Category;
    public GameObject Parent;
}

public class GiftMenu : MonoBehaviour, IItemListController
{
    [SerializeField] private CurrentlySelectedItem _currentlySelectedItem;
    [SerializeField] private RoomUIController _controller;
    [HideInInspector] public UnityEvent<(ID, ItemData)> OnGiveGift = new UnityEvent<(ID, ItemData)>();
    [SerializeField] private TextMeshProUGUI _categoryTitle;
    [SerializeField] private List<CategoryGameObjectLink> _categoryParents = new List<CategoryGameObjectLink>();

    [Header("Item List")]
    [SerializeField] private int _numItems = 16;
    [SerializeField] private Transform _gridParent;

    private ItemData _currentItem;
    private ID _id;
    private List<ItemData> _items = new List<ItemData>();
    private List<InventoryListItem> _listItems = new List<InventoryListItem>();
    private ItemType _currentCategory;
    private ItemSubType _currentSubcategory;

    public void ShowClothing() => ChangeCategory(ItemType.Clothing);
    public void ShowFood() => ChangeCategory(ItemType.Food);
    public void ShowHousing() => ChangeCategory(ItemType.Housing);

    private void OnEnable()
    {
        _listItems = _gridParent.GetComponentsInChildren<InventoryListItem>(true).ToList();
    }

    public void Show(ID id)
    {
        _id = id;
        gameObject.SetActive(true);

        _items = TownGameManager.i.GetInventoryItems();
        if (_items.Count > 0) {
            _currentCategory = _items[0].Type;
        }
        else {
            _items = new List<ItemData>();
        }
        _currentSubcategory = ItemSubType.ALL;
        DisplayItems();

        ShowClothing();
    }

    public void SelectSubcategoryAll() => ChangeSubCategory(ItemSubType.ALL);
    public void SelectSubcategoryHat() => ChangeSubCategory(ItemSubType.HAT);
    public void SelectSubcategoryTop() => ChangeSubCategory(ItemSubType.TOP);
    public void SelectSubcategoryBottom() => ChangeSubCategory(ItemSubType.BOTTOM);
    public void SelectSubcategoryShoes() => ChangeSubCategory(ItemSubType.SHOES);
    public void SelectSubcategoryOutfit() => ChangeSubCategory(ItemSubType.OUTFIT);
    public void SelectSubcategoryBreakfast() => ChangeSubCategory(ItemSubType.BREAKFAST);
    public void SelectSubcategoryLunch() => ChangeSubCategory(ItemSubType.LUNCH);
    public void SelectSubcategoryDinner() => ChangeSubCategory(ItemSubType.DINNER);
    public void SelectSubcategoryDessert() => ChangeSubCategory(ItemSubType.DESSERT);
    public void SelectSubcategoryDrinks() => ChangeSubCategory(ItemSubType.DRINKS);
    public void SelectSubcategorySnacks() => ChangeSubCategory(ItemSubType.SNACKS);
    public void SelectSubcategoryRoof() => ChangeSubCategory(ItemSubType.ROOF);
    public void SelectSubcategoryFloor() => ChangeSubCategory(ItemSubType.FLOOR);
    public void SelectSubcategoryWalls() => ChangeSubCategory(ItemSubType.WALLS);
    public void SelectSubcategoryFurniture() => ChangeSubCategory(ItemSubType.FURNITURE);
    public void ChangeSubCategory(ItemSubType subcategory)
    {
        _currentSubcategory = subcategory;
        DisplayItems();
    }

    private void DisplayItems()
    {
        _categoryTitle.text = Utils.CapitalFirst(_currentCategory.ToString());

        foreach (var c in _categoryParents) c.Parent.SetActive(c.Category == _currentCategory);

        var selectedItemArray = _items.OrderBy(x => x.Name).Where(x => x.Type == _currentCategory);
        var selectedItems = selectedItemArray.ToList();
        if (_currentSubcategory != ItemSubType.ALL) {
            selectedItems = selectedItems.Where(x => x.SubType == _currentSubcategory).ToList();
        }
        selectedItems = selectedItems.Take(_numItems).ToList();

        for (int i = 0; i < _listItems.Count; i++) {
            if (i < selectedItems.Count()) {
                _listItems[i].Initialize(selectedItems[i], this);
            }
            else {
                _listItems[i].Clear();
            }
            _listItems[i].SetDisabled(i >= selectedItems.Count());
        }
    }

    void IItemListController.SelectItem(ItemData item)
    {
        _currentItem = item;
        foreach (var i in _listItems) if (i.Item != item) i.Deselect();
        _currentlySelectedItem.ShowItem(item);
    }

    public void GiveGift()
    {
        TownGameManager.i.SubtractInventory(_currentItem);
        gameObject.SetActive(false);
        CharacterManager.i.GiveItem(_id, _currentItem);
        _controller.GiveGift(_currentItem);
        OnGiveGift.Invoke((_id, _currentItem));
    }

    private void ChangeCategory(ItemType type)
    {
        _currentCategory = type;
        DisplayItems();
    }
}

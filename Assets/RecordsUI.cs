using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecordsUI : MonoBehaviour, IItemListController
{
    [SerializeField] private ItemListDisplay _itemListDisplay;
    [SerializeField] private CurrentlySelectedItem _currentlySelectedItem;
    [SerializeField] private GameObject _currentlySelectedParent;
    [SerializeField] private TextMeshProUGUI _completionText;
    [SerializeField] private Slider _completionSlider;

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
        if (!TownGameManager.i) return;

        _currentlySelectedParent.SetActive(false);

        var items = TownGameManager.i.GetAllItems(false).ToList();
        items = items.OrderByDescending(x => TownGameManager.i.IsUnlocked(x)).ThenBy(x => !TownGameManager.i.IsAlreadyOwned(x)).ToList();
        _itemListDisplay.DisplayItems(items, this);

        var numUnlocked = items.Count(x => TownGameManager.i.IsUnlocked(x));
        var percent = (float)numUnlocked / items.Count();
        _completionText.text = "Completion: " + (Mathf.FloorToInt(percent * 100)) + "%";
        _completionSlider.value = percent;  

        ShowClothing();
    }

    private void ChangeCategory(ItemType type)
    {
        _currentlySelectedParent.SetActive(false);
        _itemListDisplay.ShowSelected(x => x.Type == type);
        _itemListDisplay.SetFirstSelected();
    }
}

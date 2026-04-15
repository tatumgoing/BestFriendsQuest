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

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        _title.text = "A gift for " + CharacterManager.i.GetName(id) + "?";

        var items = TownGameManager.i.GetInventoryItems();
        _itemListDisplay.DisplayItem(items, this);
    }

    void IItemListController.SelectItem(ItemData item)
    {
        _itemListDisplay.DeselectNonMatching(item);
        _currentlySelectedItem.ShowItem(item);
    }

    public void GiveGift()
    {
        /*if (giftManager.selectedBanner != null && giftManager.selectedBanner.itemCount.text != "0") {
            bool passed = false;
            if (associatedCharacter.HasProblem && !associatedCharacter.CurrentProblem.IsMinigame) {
                foreach (Item checkItem in associatedCharacter.CurrentProblem.desiredItem) {
                    if (checkItem.Name == giftManager.selectedBanner.itemID.Name) {
                        StartCoroutine(SolveProblem());
                        passed = true;
                    }
                }
                if (!passed) {
                    StartCoroutine(FailProblem());
                }
            }
            else {
                StartCoroutine(RecieveGift());
            }

            TownGameManager.i.SubtractInventory(giftManager.selectedBanner.itemID);
            TownGameManager.i.UpdateRecordDisplay(giftManager, giftManager.currentType);
        }*/
    }

}

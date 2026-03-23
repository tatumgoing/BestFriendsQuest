using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftMenu : MonoBehaviour
{
    [Header("Gift Inventory")]
    public RecordsManager giftManager;

    public void Show()
    {
        gameObject.SetActive(true);
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

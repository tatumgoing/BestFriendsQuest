using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResetter : MonoBehaviour
{
    [ButtonMethod]
    public void ResetAll()
    {
        TownGameManager.i.SetCurrency(50);
        TownGameManager.i.ResetInventory();
        SaveSystem.ResetCompletedBFQuests();
        SaveSystem.ResetInProgressQuest();
        SaveSystem.ResetRegion();
        SaveSystem.ResetDialogueDict();
    }
}

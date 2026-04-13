using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    TownGameManager gameManager;

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
        gameManager = TownGameManager.i;

        foreach (ItemTabs tab in tabs)
        {
            tab.GetComponent<Button>().onClick.AddListener(() => UpdateTab(tab));
        }

        UpdateTab(tabs[0]);
        purchaseButton.GetComponent<Button>().onClick.AddListener(() => UpdatePurchasedButton());

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

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

    [Header("Selection Menu")]
    public RecordsManager recordsManager;
    public List<ItemTabs> tabs = new List<ItemTabs>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = TownGameManager.i;

        foreach (ItemTabs tab in tabs)
        {
            tab.GetComponent<Button>().onClick.AddListener(() => UpdateTab(tab));
        }

        UpdateTab(tabs[0]);
    }

    private void Update()
    {
        
        if (recordsManager.selectedBanner != null)
        {
            if (gameManager.items.ContainsKey(recordsManager.selectedBanner.itemID))
            {

                descriptionContainer.SetActive(true);

                UpdateDescription(recordsManager.selectedBanner.itemID.Description);
                UpdateCurrentlyHeld(gameManager.items[recordsManager.selectedBanner.itemID]);
            }
            else
            {
                descriptionContainer.SetActive(true);

                UpdateDescription(recordsManager.selectedBanner.itemID.Description);
                UpdateCurrentlyHeld(0);
            }
        }
        else
        {
            descriptionContainer.SetActive(false);
        }
        
    }


    // Update is called once per frame
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

    void UpdateDescription(string newDesc)
    {
        descriptionTextbox.text = newDesc;
    }

    void UpdateCurrentlyHeld(int newCount)
    {
        currentHeldTextbox.text = "Currently Held: " + newCount.ToString();
    }


}

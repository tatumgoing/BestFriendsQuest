using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsManager : MonoBehaviour
{
    TownGameManager gameManager;

    public bool isRecords;

    public ItemBanner heldItem;
    public ItemBanner unheldItem;
    public ItemBanner lockedItem;

    public List<ItemBanner> containedItems = new List<ItemBanner>();

    public ItemBanner selectedBanner;

    [Header("Item Type")]

    public List<ItemTabs> tabs = new List<ItemTabs>();
    public ItemType currentType;

    void Start()
    {
        gameManager = TownGameManager.i;

        StartCoroutine(UpdateRecord());

        currentType = ItemType.Clothing;

        foreach (ItemTabs tab in tabs) { 
            tab.GetComponent<Button>().onClick.AddListener(() => UpdateType(tab.type));
            tab.GetComponent<Button>().onClick.AddListener(() => UpdateTab(tab));
        }

        UpdateTab(tabs[0]);
    }

    public IEnumerator UpdateRecord()
    {
        yield return new WaitForEndOfFrame();

        gameManager.UpdateRecordDisplay(this, currentType);

        foreach (ItemBanner i in containedItems)
        {
            if (i.GetComponent<Button>() != null)
            {
                i.GetComponent<Button>().onClick.AddListener(() => SelectBanner(i));
            }
        }
    }

    public void UpdateRecordSync()
    {
        foreach (ItemBanner i in containedItems)
        {
            if (i.GetComponent<Button>() != null)
            {
                i.GetComponent<Button>().onClick.AddListener(() => SelectBanner(i));
            }
        }
    }

    public void ClearRecords()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);

            containedItems = new List<ItemBanner>();

            selectedBanner = null;
        }
    }

    public void CreateHeldItem(ItemData item, int itemCount, float price)
    {

       ItemBanner newBanner = Instantiate(heldItem, this.transform);
       newBanner.UpdateBanner(item);
       newBanner.UpdateCount(itemCount);
       newBanner.UpdatePrice(price);

       containedItems.Add(newBanner);

    }

    public void CreateUnheldItem(ItemData item, int itemCount, float price)
    {
        if (isRecords) { 
            CreateHeldItem(item, itemCount, item.Cost);
        }
        else
        {
            ItemBanner newBanner = Instantiate(unheldItem, this.transform);
            newBanner.UpdateBanner(item);
            newBanner.UpdateCount(itemCount);
            newBanner.UpdatePrice(price);

            containedItems.Add(newBanner);
        }
    }

    public void CreateLockedItem(ItemData item) {

        if (isRecords) {
            ItemBanner newBanner = Instantiate(lockedItem, this.transform);
            newBanner.UpdateName("???");
            newBanner.UpdateCount(0);
            containedItems.Add(newBanner);
        }


    }

    public void SelectBanner(ItemBanner newSelected)
    {
        selectedBanner = newSelected;
        //Debug.Log("Yay new selected: " + newSelected);
    }

    public void UpdateType(ItemType type)
    {
        currentType = type;
        gameManager.UpdateRecordDisplay(this, type);
    }
    void UpdateTab(ItemTabs clickedTab)
    {
        foreach (ItemTabs tab in tabs)
        {
            if (tab != clickedTab)
            {
                tab.selected = false;
            }
        }

        clickedTab.selected = true;
    }
    //go back to towngamemanager and update item class to accomodate. sorry future vincent
}

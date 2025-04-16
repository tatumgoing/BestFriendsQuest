using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsManager : MonoBehaviour
{
    TownGameManager gameManager;

    public bool isRecords;

    public string listType;

    public ItemBanner heldItem;
    public ItemBanner unheldItem;
    public ItemBanner lockedItem;

    public List<ItemBanner> containedItems = new List<ItemBanner>();

    public ItemBanner selectedBanner;


    //public GameObject recordContainer;

    // Start is called before the first frame update
    void OnEnable()
    {
        //StartCoroutine(UpdateRecord());
    }
    void Start()
    {
        gameManager = TownGameManager.i;

        StartCoroutine(UpdateRecord());
    }

    public IEnumerator UpdateRecord()
    {
        yield return new WaitForSeconds(.05f);
        gameManager.UpdateRecords(this);

        foreach (ItemBanner i in containedItems)
        {
            i.GetComponent<Button>().onClick.AddListener(() => SelectBanner(i));
        }
    }
    public void ClearRecords()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void CreateHeldItem(Item item, int itemCount)
    {

       ItemBanner newBanner = Instantiate(heldItem, this.transform);
       newBanner.UpdateBanner(item);
       newBanner.UpdateCount(itemCount);
       containedItems.Add(newBanner);

    }

    public void CreateUnheldItem(Item item, int itemCount)
    {
        if (isRecords) { 
            CreateHeldItem(item, itemCount);
        }
        else
        {
            ItemBanner newBanner = Instantiate(unheldItem, this.transform);
            newBanner.UpdateBanner(item);
            newBanner.UpdateCount(itemCount);
            containedItems.Add(newBanner);
        }
    }

    public void CreateLockedItem(Item item) {

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

    //go back to towngamemanager and update item class to accomodate. sorry future vincent

}

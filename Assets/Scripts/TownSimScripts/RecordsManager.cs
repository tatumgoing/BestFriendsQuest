using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsManager : MonoBehaviour
{
    TownGameManager gameManager;

    public bool isRecords;

    public string listType;

    public ItemBanner heldItem;
    public ItemBanner unheldItem;
    public ItemBanner lockedItem;


    //public GameObject recordContainer;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(UpdateRecord());
    }
    void Start()
    {
        gameManager = TownGameManager.i;
    }

    public IEnumerator UpdateRecord()
    {
        yield return new WaitForSeconds(.05f);
        gameManager.UpdateRecords(this);
    }
    public void ClearRecords()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void CreateHeldItem(string itemName, int itemCount)
    {

       ItemBanner newBanner = Instantiate(heldItem, this.transform);
       newBanner.UpdateName(itemName);
       newBanner.UpdateCount(itemCount);

    }

    public void CreateUnheldItem(string itemName, int itemCount)
    {
        if (isRecords) { 
            CreateHeldItem(itemName, itemCount);
        }
        else
        {
            ItemBanner newBanner = Instantiate(unheldItem, this.transform);
            newBanner.UpdateName(itemName);
            newBanner.UpdateCount(itemCount);
        }
    }

    public void CreateLockedItem(string itemName) {

        if (isRecords) {
            ItemBanner newBanner = Instantiate(lockedItem, this.transform);
            newBanner.UpdateName("???");
            newBanner.UpdateCount(0);
        }
        
        
    }

    //go back to towngamemanager and update item class to accomodate. sorry future vincent

}

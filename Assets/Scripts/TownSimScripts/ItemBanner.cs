using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemBanner : MonoBehaviour
{
    public Item itemID;
    // Start is called before the first frame update
    public TMP_Text itemName;
    public TMP_Text itemCount;

    
    public void UpdateBanner(Item item)
    {
        UpdateName(item.Name);
        itemID = item; 
    }
    public void UpdateName(string newName)
    {
        itemName.text = newName;
    }

    public void UpdateCount(int newCount)
    {
        itemCount.text = newCount.ToString();
    }
}

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
    public TMP_Text itemPrice;

    
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
        if(itemCount != null)
        {
            itemCount.text = newCount.ToString();
        }
    }

    public void UpdatePrice(float newPrice)
    {
        if (itemPrice != null)
        {
            itemPrice.text = "$" + newPrice.ToString("F2");
        }
    }
}

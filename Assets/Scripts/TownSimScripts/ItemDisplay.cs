using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemDisplay : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text priceText;

    public bool selected;

    public void SetItemDisplay(string newName, string newPrice)
    {
        nameText.text = newName;
        priceText.text = newPrice;
    }
}

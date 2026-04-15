using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlySelectedShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _iconImg;

    public void Initialize(ItemData item)
    {
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        _priceText.text = item.Cost.ToString();
        _iconImg.sprite = item.sprite;
    }
}

using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlySelectedItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _iconImg;
    [SerializeField] private bool _showPrice;
    [SerializeField, ConditionalField(nameof(_showPrice))] private TextMeshProUGUI _priceText;

    public void ShowItem(ItemData item)
    {
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        _iconImg.sprite = item.sprite;
        if (_showPrice) _priceText.text = item.Cost.ToString();
    }
}

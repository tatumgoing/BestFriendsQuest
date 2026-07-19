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
    [SerializeField] private bool _recordsMode;
    [SerializeField] private Material _greyscaleMaterial;

    public void ShowItem(ItemData item)
    {
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        _iconImg.sprite = item.sprite;
        if (_showPrice) _priceText.text = item.Cost.ToString();

        if (_recordsMode) {
            _iconImg.material = new Material(_greyscaleMaterial);
            bool unlocked = TownGameManager.i.IsUnlocked(item);
            if (!unlocked) {
                _iconImg.color = Color.black;
                _nameText.text = "???";
                _descriptionText.text = "Unknown item. Go on more Best Friend Quests to discover...";
            }
            else _iconImg.color = Color.white;

            _iconImg.material.SetFloat("_GrayscaleAmount", TownGameManager.i.IsAlreadyOwned(item) ? 0 : 1);
        }
    }
}

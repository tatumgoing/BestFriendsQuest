using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopListItem : MonoBehaviour
{
    [SerializeField] private SelectableItem _buttonScript;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private ItemData _item;
    private ShopUIController _controller;

    public ItemData Item => _item;

    public void Deselect() => _buttonScript.Deselect(true, false);

    public void Initialize(ItemData item, ShopUIController controller)
    {
        _controller = controller;
        _item = item;
        _nameText.text = item.Name;
        _priceText.text = item.Cost.ToString();
    }

    public void Select()
    {
        _controller.SelectItem(_item);
    }
}

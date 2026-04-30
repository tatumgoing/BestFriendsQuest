using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopListItem : MonoBehaviour, IListIItem
{
    [SerializeField] private SelectableItem _buttonScript;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private ItemData _item;
    private IItemListController _controller;

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
    public ItemData Item => _item;
    public bool Active => gameObject.activeInHierarchy;
    public void Deselect() => _buttonScript.Deselect(true, false);
    void IListIItem.Destroy() => Destroy(gameObject);
    public void Select() => _controller.SelectItem(_item);
    public void SetSelected() => _buttonScript.Select(true);

    public void Initialize(ItemData item, IItemListController controller)
    {
        _controller = controller;
        _item = item;
        _nameText.text = item.Name;
        _priceText.text = item.Cost.ToString();
    }

}

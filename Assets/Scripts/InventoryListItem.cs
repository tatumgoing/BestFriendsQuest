using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryListItem : MonoBehaviour, IListIItem
{
    [SerializeField] private SelectableItem _buttonScript;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Image _icon;
    [SerializeField] private bool _records;
    [SerializeField] private Material _greyscaleMaterial;
     
    private ItemData _item;
    private IItemListController _controller;

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
    public ItemData Item => _item;
    public bool Active => gameObject.activeInHierarchy;
    public void Deselect() => _buttonScript.Deselect(true, false);
    public void SetDisabled(bool disabled) => _buttonScript.SetDisabled(disabled);
    void IListIItem.Destroy() => Destroy(gameObject);
    public void Select() => _controller.SelectItem(_item);
    public void SetSelected() => _buttonScript.Select(true);
    public void SetActive(bool active) => gameObject.SetActive(active);

    public void Clear()
    {
        _item = null;
        if (_nameText) _nameText.text = "";
        if (_quantityText) _quantityText.text = "";
        if (_icon) _icon.enabled = false;
    }

    public void Initialize(ItemData item, IItemListController controller)
    {
        _controller = controller;
        _item = item;
        if (_nameText) _nameText.text = item.Name;
        if (_quantityText) _quantityText.text = TownGameManager.i.GetNumberOwned(item).ToString();
        if (_icon) {
            _icon.sprite = item.sprite;
            _icon.enabled = true;
            if (_greyscaleMaterial) _icon.material = new Material(_greyscaleMaterial);
        }
        

        bool unlocked = TownGameManager.i.IsUnlocked(item);
        if (!unlocked) {
            if (_icon)_icon.color = Color.black;
            if (_nameText) _nameText.text = "???";
        }

        if (_icon && _greyscaleMaterial) {
             _icon.material.SetFloat("_GrayscaleAmount", TownGameManager.i.IsAlreadyOwned(item) ? 0 : 1);
        }
    }
}

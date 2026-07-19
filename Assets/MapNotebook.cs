using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class LocationNotebookData
{
    public string DisplayName;
    [TextArea(3,10)]public string Description;
    public AreaName Area;
}

public class MapNotebook : MonoBehaviour
{
    [SerializeField] private List<LocationNotebookData> _data = new List<LocationNotebookData>();
    [SerializeField] private string _defaultName;
    [SerializeField, TextArea(3,10)] private string _defaultDescription;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private GameObject _inventoryButton;
    [SerializeField] private Animator _animtor;
    [SerializeField] private StickerRandomizer _stickers;
    [SerializeField] private Vector2 _rotLimits = new Vector2(-2, 2);
    [SerializeField] private Sound _hoverSound;

    private float _originalZ;

    private void Start()
    {
        _originalZ = transform.localEulerAngles.z;
        _hoverSound = Instantiate(_hoverSound);
    }

    public void EndHover()
    {
        _nameText.text = _defaultName;
        _descriptionText.text = _defaultDescription;
        _inventoryButton.SetActive(true);
        _animtor.SetTrigger("Throb");
        _stickers.Randomize();
        transform.localEulerAngles = Vector3.forward * _originalZ;
        _hoverSound.Play();
    }

    public void StartHoverFarm() => StartHover(AreaName.FARM);
    public void StartHoverGrocery() => StartHover(AreaName.GROCERY_STORE);
    public void StartHoverHardware() => StartHover(AreaName.HARDWARE_STORE);
    public void StartHoverLake() => StartHover(AreaName.LAKE);
    public void StartHoverPark() => StartHover(AreaName.PARK);
    public void StartHoverPort() => StartHover(AreaName.PORT);
    public void StartHoverRestaurant() => StartHover(AreaName.RESTURAUNT);
    public void StartHoverClothes() => StartHover(AreaName.SHOP);
    public void StartHoverTownHall() => StartHover(AreaName.TOWN_HALL);
    public void StartHoverTown() => StartHover(AreaName.TOWN);
    public void StartHover(AreaName area)
    {
        var selected = _data.Where(x => x.Area == area).ToList();
        if (selected.Count == 0) {
            EndHover();
            return;
        }

        _nameText.text = selected[0].DisplayName;
        _descriptionText.text = selected[0].Description;
        _inventoryButton.SetActive(false);
        _animtor.SetTrigger("Throb");
        _stickers.Randomize();

        var newZ = _originalZ + Utils.Rand(_rotLimits);
        transform.localEulerAngles = Vector3.forward * newZ;
        _hoverSound.Play();
    }
}

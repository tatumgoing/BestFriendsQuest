using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NavButtonData
{
    [SerializeField] private AreaName _name;
    [SerializeField] private SelectableItem _button;

    public void UpdateUnlocked()
    {
        if (!DemoController.i || !_button) return;

        var isUnlocked = DemoController.i.IsUnlocked(_name);
        _button.SetDisabled(!isUnlocked);
    }
}

public class MapUIController : MonoBehaviour
{
    [SerializeField] private GameObject _initialBacking;
    [SerializeField] private GameObject _closeButton;

    [SerializeField] private List<NavButtonData> _buttonData = new List<NavButtonData>();

    private void OnEnable()
    {
        _closeButton.SetActive(!_initialBacking.activeInHierarchy);

        UpdateUnlocked();
    }

    private void UpdateUnlocked()
    {
        foreach (var b in _buttonData) b.UpdateUnlocked();
    }
}

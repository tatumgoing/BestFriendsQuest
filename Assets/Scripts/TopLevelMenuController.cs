using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLevelMenuController : MonoBehaviour
{
    [SerializeField] private List<SelectableItem> _tabButtons = new List<SelectableItem>();
    [SerializeField] private List<GameObject> _menus = new List<GameObject>();
    [SerializeField] private LayersMenuController _layersMenu;

    private void OnEnable()
    {
        if (_layersMenu) _layersMenu.Initialize();

        if (_tabButtons[0]) _tabButtons[0].Select();
        else SelectTab(0);
    }

    public void SelectTab(int tab)
    {
        //print(gameObject.name + " selecting tab " + tab);

        if (_tabButtons[0] && tab > 0 && _layersMenu && !_layersMenu.HasCurrent()) {
            var button = _tabButtons[tab];
            if (button) button.Deselect();
            return;
        }

        for (int i = 0; i < _tabButtons.Count; i++) {
            var button = _tabButtons[i];
            if (i != tab && button) button.Deselect();
            _menus[i].SetActive(i == tab);
        }
    }
}

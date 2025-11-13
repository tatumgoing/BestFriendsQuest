using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HairMenu : MonoBehaviour
{
    [SerializeField] private List<Animator> _tabAnimtors = new List<Animator>();
    [SerializeField] private List<SelectableItem> _tabButtons = new List<SelectableItem>();
    [SerializeField] private GameObject _mainHairParent;
    [SerializeField] private TextMeshProUGUI _subTitleText;

    int _currentIndex = 0;

    public void SwitchToTab(int newIndex)
    {
        _subTitleText.text = newIndex == 0 ? "Style" : "Color";
        //if (newIndex == _currentIndex) return;

        for (int i = 0; i < _tabButtons.Count; i++) {
            if (i != newIndex) _tabButtons[i].Deselect(true, false);
            else _tabButtons[i].Select(false, false);
        }

        _tabAnimtors[_currentIndex].gameObject.SetActive(false);
        _tabAnimtors[newIndex].gameObject.SetActive(true);

        var dir = newIndex > _currentIndex ? "Right" : "Left";
        _tabAnimtors[newIndex].SetTrigger(dir);

        _currentIndex = newIndex;
    }

    private void Update()
    {
        //_tabButtons[1].SetDisabled(!_mainHairParent_mainHairParent.gameObject.activeInHierarchy);
    }

}

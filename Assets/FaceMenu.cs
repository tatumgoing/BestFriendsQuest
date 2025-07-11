using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FeatureCategory { EYES, EYEBROWS, NOSE, MOUTH, OTHER }
public enum TabButtonType { LAYER, POSITION, COLOR }

public class FaceMenu : MonoBehaviour
{
    [SerializeField] private TopLevelMenuController _menuController;
    [SerializeField] private TextMeshProUGUI _subtitleText;
    [SerializeField] private TextMeshProUGUI _categoryText;
    [SerializeField] private List<Animator> _tabAnimtors = new List<Animator>();
    [SerializeField] private List<SelectableItem> _tabButtons = new List<SelectableItem>();
    [SerializeField] private FaceFeatureController _featureController;
    [SerializeField] private LayersMenuController _layers;

    private int _currentIndex = 0;
    private List<TabButtonType> _buttonHistory = new List<TabButtonType>();

    private void OnEnable()
    {
        _buttonHistory.Clear();
    }

    public void AddToHistory(TabButtonType TabButtonType)
    {
        if (TabButtonType ==0) _buttonHistory.Clear();
        else if (!_buttonHistory.Contains(TabButtonType)) _buttonHistory.Add(TabButtonType);
    }

    public void BackButton()
    {
        if (_buttonHistory.Count > 0) {
            _buttonHistory.RemoveAt(_buttonHistory.Count - 1);
            if (_buttonHistory.Count == 0) _tabButtons[0].Select();
            else _tabButtons[(int)_buttonHistory[_buttonHistory.Count - 1]].Select();
        }
        else {
            GoToCategories();
        }
    }

    public void OpenCategory(FeatureCategory category)
    {
        _featureController.SetCategory(category);
        _categoryText.text = Utils.CapitalFirst(category.ToString());
        _layers.Initialize();
        
        SwitchToTab(0);
    }

    public void SwitchSelectedLayer(Layer newSelected)
    {
        _tabButtons[2].SetDisabled(newSelected.GetFeature().GetData().ColorMask == null);
    }

    public void GoToCategories()
    {
        _menuController.SelectTab(0);
    }

    public void SetSubtitleText(string text)
    {
        _subtitleText.text = text;
    }

    public void SwitchToTab(int newIndex)
    {
        AddToHistory((TabButtonType)newIndex);

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
}

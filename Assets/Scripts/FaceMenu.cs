using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FeatureCategory { EYES, EYEBROWS, NOSE, MOUTH, EXTRAS }
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

    public void OpenCategory(FeatureCategory category)
    {
        _featureController.SetCategory(category);
        _categoryText.text = Utils.CapitalFirst(category.ToString());

        //print("category: " + category);

        if (category == FeatureCategory.EYES) _layers.Initialize(FeatureSubType.EYES);
        else if (category == FeatureCategory.EYEBROWS) _layers.Initialize(FeatureSubType.BROWS);
        else if (category == FeatureCategory.NOSE) _layers.Initialize(FeatureSubType.NOSE);
        else if (category == FeatureCategory.MOUTH) _layers.Initialize(FeatureSubType.LIPS);
        else if (category == FeatureCategory.EXTRAS) _layers.Initialize(FeatureSubType.MISC);
        else _layers.Initialize(FeatureSubType.LIPS);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureCategoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject _tabs;
    [SerializeField] private TopLevelMenuController _faceMenuController;

    private void OnEnable()
    {
        _tabs.SetActive(false);
    }

    public void SelectCategory()
    {
        _tabs.SetActive(true);  
        gameObject.SetActive(false);

        _faceMenuController.SelectTab(1);
    }
}

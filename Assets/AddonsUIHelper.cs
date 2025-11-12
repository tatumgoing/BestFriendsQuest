using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddonsUIHelper : MonoBehaviour
{
    [SerializeField] private GameObject _expressionsParent;
    [SerializeField] private GameObject _nextButtonParent;
    [SerializeField] private GameObject _moveRotButonParent;
    [SerializeField] private GameObject _sliderParent;
    [SerializeField] private GameObject _rmbPanParent;
    [SerializeField] private GameObject _lmbRotParent;
    [SerializeField] private LayersMenuController _layerMenu;

    [HideInInspector] public bool Rotating;
    private bool _addons = false;

    private void Start()
    {
        UIManager.i.OnTabSwitch.AddListener(() =>
        {
            Rotating = false;
            SwitchToMain();
        });
    }

    private void Update()
    {
        UpdateVisuals();
    }

    public void SwitchToMain()
    {
        _addons = false;
        UpdateVisuals();
        if (Rotating) Rotating = false;
    }

    public void SwitchToAddons()
    {
        _addons = true;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _expressionsParent.SetActive(!_addons);
        _nextButtonParent.SetActive(!_addons);
        _lmbRotParent.SetActive(!_addons);

        _moveRotButonParent.SetActive(_addons && _layerMenu.NumLayers > 0);
        _rmbPanParent.SetActive(_addons);
    }

    public void SwitchToRotate() => Rotating = true;
    public void SwitchToMove() => Rotating = false;
}

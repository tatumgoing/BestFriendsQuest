using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddonsUIHelper : MonoBehaviour
{
    [SerializeField] private Animator _expressionsParent;
    [SerializeField] private Animator _lmbRotParent;
    [SerializeField] private Animator _nextButtonParent;

    [SerializeField] private Animator _rmbPanParent;
    [SerializeField] private GameObject _moveRotButonParent;
    [SerializeField] private LayersMenuController _layerMenu;
    [SerializeField] private DragToSpin _dragToSpin;

    [HideInInspector] public bool Rotating;
    private bool _addons = false;

    public bool Addons => _addons;

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

    public void SetSize(float size)
    {
        _layerMenu.SetScale(size);
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
        if (_addons) {
            _expressionsParent.SetTrigger("Exit");
            _lmbRotParent.SetTrigger("Exit");
            _nextButtonParent.SetTrigger("Exit");

            _rmbPanParent.gameObject.SetActive(true);
        }
        else {
            _expressionsParent.GetComponent<ExpressionButtonsController>().Show();
            _lmbRotParent.gameObject.SetActive(true);
            _nextButtonParent.gameObject.SetActive(true);

            _rmbPanParent.SetTrigger("Exit");
        }

        _dragToSpin.enabled = !_addons;


        _moveRotButonParent.SetActive(_addons && _layerMenu.NumLayers > 0);
    }

    public void SwitchToRotate() => Rotating = true;
    public void SwitchToMove() => Rotating = false;
}

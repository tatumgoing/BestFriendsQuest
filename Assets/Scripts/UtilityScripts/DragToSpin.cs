using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class DragToSpin : MonoBehaviour
{
    [SerializeField] private float _dragSpeed = 10;
    [SerializeField] private float _lerpFactor = 10;

    private float _rotDelta = 0;

    private bool _beingDragged;
    private bool _hovered;

    private Quaternion _originalRot;

    private void Start()
    {
        UIManager.i.OnTabSwitch.AddListener(() => enabled = true);
        _originalRot = transform.localRotation;
    }

    private void Update()
    {
        UpdateHovered();
        if (_hovered && Input.GetMouseButtonDown(0)) StartDrag();
        if (Input.GetMouseButtonUp(0) && _beingDragged) EndDrag();
        if (_beingDragged) Drag();
        else _rotDelta = Mathf.Lerp(_rotDelta, 0, _lerpFactor * Time.deltaTime);

        transform.Rotate(Vector3.up * -_rotDelta * 10 * Time.deltaTime * _dragSpeed);
    }

    public void Reset()
    {
        transform.localRotation = _originalRot;
        _rotDelta = 0;
    }

    private void EndDrag()
    {
        _beingDragged = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void StartDrag()
    {
        _beingDragged = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Drag()
    {
        var mouseDelta = Input.GetAxis("Mouse X");
        _rotDelta = Mathf.Lerp(_rotDelta, mouseDelta, _lerpFactor * Time.deltaTime);
    }

    private void UpdateHovered()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            _hovered = false;
            return;
        }

        var raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(raycast, out var hitInfo);
        if (hit) _hovered = hitInfo.collider.GetComponentInParent<DragToSpin>() == this;
        else _hovered = false;

        //print("hit: " + hit);
        //if (hit) print("hitData: " + hitInfo.collider.name);
    }
}

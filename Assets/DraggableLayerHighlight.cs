using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DraggableLayerHighlight : MonoBehaviour
{
    [SerializeField] private float _yScale = 0.05f;
    [SerializeField] private float _yScaleHidden = 0.05f;
    [SerializeField] private float _minDist = 110f;
    [SerializeField] private GameObject _img;
    [SerializeField] private Transform _spacer;

    [SerializeField, ReadOnly] private int _disallowed = -1;
    [SerializeField, ReadOnly] private int _targetIndex = -1;
    private RectTransform _rTransform;
    private Layer _dragged;

    private void OnEnable()
    {
        _disallowed = -1;
    }

    private void Start()
    {
        _rTransform = GetComponent<RectTransform>();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("TypeSafety", "UNT0006:Incorrect message signature", Justification = "fixed flashing error on click. still works?")]
    public async Task Update()
    {
        if (Input.GetMouseButtonUp(0) && _dragged) {
            _dragged.transform.SetSiblingIndex(_targetIndex);

            transform.SetSiblingIndex(transform.parent.childCount-2);
            _spacer.SetAsLastSibling();

            foreach (Transform child in transform.parent) {
                var layer = child.GetComponent<Layer>();
                if (layer) layer.UpdatePriority(child.GetSiblingIndex());
            }
        }

        var scale = transform.localScale;
        scale.y = Input.GetMouseButton(0) && transform.parent.childCount > 2 ? _yScale : _yScaleHidden;
        transform.localScale = scale;

        var hidden = transform.localScale.y < _yScale;
        _img.SetActive(!hidden);
        if (hidden) {
            transform.SetAsLastSibling();
            return;
        }

        await Task.Delay(100);

        var siblingIndex = transform.GetSiblingIndex();
        _targetIndex = siblingIndex;
        var mouseDist = GetMouseDist();
        if (Mathf.Abs(mouseDist) < _minDist) return;

        if (mouseDist > 0 && siblingIndex > 0) {
            _targetIndex = siblingIndex - 1;
            if (_targetIndex == _disallowed && _targetIndex > 0) _targetIndex--;
        }
        else if (mouseDist < 0 && siblingIndex < transform.parent.childCount - 2) {
            _targetIndex = siblingIndex + 1;
            if (_targetIndex == _disallowed && _targetIndex < transform.parent.childCount - 2) _targetIndex++;
        }

        if (_targetIndex != _disallowed && _targetIndex != siblingIndex) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            transform.SetSiblingIndex(_targetIndex);
        }

    }

    public void StartDrag(int newDisallowed, Layer dragged)
    {
        _targetIndex = newDisallowed;
        transform.SetSiblingIndex(newDisallowed);
        _disallowed = newDisallowed;
        _dragged = dragged;
    }

    private float GetMouseDist()
    {
        Vector2 mousePos = Input.mousePosition;

        var center = RectTransformUtility.WorldToScreenPoint(null, _rTransform.TransformPoint(_rTransform.rect.center));
        return mousePos.y - center.y;
    }
}

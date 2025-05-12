using MyBox.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FollowMouseInRectBounds : MonoBehaviour
{
    [SerializeField] private RectTransform _bounds;
    [SerializeField] private Vector2 _offset;

    private RectTransform _rTransform;
    public bool FollowMouse;
    private Image _image;
    

    private void OnEnable()
    {
        if (!_image) _image = GetComponent<Image>();
        if (!_rTransform) _rTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (FollowMouse) transform.position = Input.mousePosition;
        _image.enabled = FollowMouse;
    }

    private void LateUpdate()
    {
        var pos = _rTransform.anchoredPosition;
        float width = _bounds.sizeDelta.x / 2;
        float height = _bounds.sizeDelta.y / 2;
        pos.x = Mathf.Clamp(pos.x, -width + _offset.x, width + _offset.x);
        pos.y = Mathf.Clamp(pos.y, -height + _offset.y, height + _offset.y);
        _rTransform.anchoredPosition = pos;
    }

    public Vector2 GetNormalizedPositionFromCenter()
    {
        if (!_rTransform) _rTransform = GetComponent<RectTransform>();

        var oldPos = transform.position;
        if (!FollowMouse) {
            transform.position = Input.mousePosition;
        }

        float x = _rTransform.anchoredPosition.x / _bounds.sizeDelta.x;
        float y = _rTransform.anchoredPosition.y / _bounds.sizeDelta.y;
        var pos = new Vector2(x + 0.5f, y + 0.5f);
  

        if (!FollowMouse) transform.position = oldPos;

        return pos;
    }

    public void SetPosition(Vector2 pos)
    {
        if (_rTransform == null) _rTransform = GetComponent<RectTransform>();
        pos.x -= 0.5f;
        pos.y -= 0.5f;

        var x = pos.x * _bounds.sizeDelta.x;
        var y = pos.y * _bounds.sizeDelta.y;

        _rTransform.anchoredPosition = new Vector2(x, y);
    }
}

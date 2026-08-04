using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborhoodScrollInstructions : MonoBehaviour
{
    [SerializeField] private float _maxDist;
    [SerializeField] private float _speed;
    [SerializeField] private float _lerpFactor;
    [SerializeField] private NeighborhoodCamera _camera;

    private RectTransform _rTransform;
    private float _currentDist;
    private float moveDelta;

    private void Start()
    {
        _rTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (_camera.IsFocused ? 0 : 0.7f), Time.deltaTime * 5f);
        if (_camera.IsFocused) return;


        var scrollDelta = Input.mouseScrollDelta.y;
        if ( Mathf.Abs(moveDelta) < 0.1f && Mathf.Abs(scrollDelta) < 0.01f && Mathf.Abs(_currentDist) > 3) {
            scrollDelta = -1 * Mathf.Sign(_currentDist);
        }
        var targetDelta = scrollDelta * _speed * Time.deltaTime;       

        if (_currentDist + targetDelta > _maxDist) {
            targetDelta = _maxDist - _currentDist;
        }
        if (_currentDist + targetDelta < -_maxDist)  {
            targetDelta = -_maxDist - _currentDist;
        }

        moveDelta = Mathf.Lerp(moveDelta, targetDelta, _lerpFactor * Time.deltaTime);
        _rTransform.anchoredPosition += new Vector2(0, moveDelta);
        _currentDist += moveDelta;
    }
}

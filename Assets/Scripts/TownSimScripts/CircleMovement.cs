using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    [SerializeField] private float _speedLerpFactor = 10;
    [SerializeField] private float _radius = 425;
    [SerializeField] private Vector2 _centerPoint = new Vector2(0, 0);

    private float _currentAngle;
    private float _currentSpeed;
    private RectTransform _rectTransform;
    [SerializeField] private float _targetSpeed;

    public void SetMoveSpeed(float newSpeed) => _targetSpeed = newSpeed;
    public Vector2 Position => new Vector2(_rectTransform.position.x, _rectTransform.position.y);

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * _speedLerpFactor);

        _currentAngle += _currentSpeed * Time.deltaTime;

        // Calculate the new position using trigonometry
        float x = _centerPoint.x + Mathf.Cos(_currentAngle) * _radius;
        float y = _centerPoint.y + Mathf.Sin(_currentAngle) * _radius;

        // Update the RectTransform position
        _rectTransform.anchoredPosition = new Vector2(x, y);
    }
}

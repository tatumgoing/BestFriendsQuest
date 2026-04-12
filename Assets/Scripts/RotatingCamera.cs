using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
public class RotatingCamera : MonoBehaviour
{
    [SerializeField] private float _topSpeed = 500;
    [SerializeField] private float _lerpFactor = 10;

    private float _currentSpeed;
    private float _targetSpeed;

    void Update()
    {
        _targetSpeed = 0;
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            } 
            _targetSpeed = Input.GetAxis("Mouse X");
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * _lerpFactor);
        var rotDelta = Mathf.Clamp(_currentSpeed, -1, 1) * _topSpeed;
        transform.localEulerAngles += rotDelta * Vector3.up;
    }
}

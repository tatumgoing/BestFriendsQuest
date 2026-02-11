using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeighborhoodCamera : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _rotationFriction = 10;

    [Header("zoom")]
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _zoomFriction = 10;
    [SerializeField] private float _zoomTiltFriction = 10;
    [SerializeField] private Vector2 _zoomHeightLimits;
    [SerializeField] private Vector2 _zoomAngleLimits;

    [Header("Pan")]
    [SerializeField] private float _panSpeed = 3;
    [SerializeField] private float _panFriction = 10;
    [SerializeField] private Vector2 _panLimits;


    private float _deltaRot;
    private float _deltaZoom;
    private Vector3 _deltaPos;
    private Vector3 _startingPosition;

    private void Start()
    {
        _startingPosition = transform.localPosition;
    }

    void Update()
    {
        Rot();
        Zoom();
        Tilt();
        Pan();
    }

    private void Pan()
    {
        var forward = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0;
        var left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0;
        var back = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0;
        var right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;
        var dir = new Vector3(right - left, 0, forward - back ).normalized;

        _deltaPos = Vector3.Lerp(_deltaPos, dir, _panFriction * Time.deltaTime);
        transform.localPosition += transform.TransformDirection(_deltaPos * _panSpeed);

        var pos = transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, _startingPosition.x - _panLimits.x, _startingPosition.z + _panLimits.x);
        pos.z = Mathf.Clamp(pos.z, _startingPosition.z - _panLimits.y, _startingPosition.z + _panLimits.y);
        transform.localPosition = pos;
    }

    private void Zoom()
    {
        var targetZoom = 0f;
        if (Input.mouseScrollDelta.y != 0) {
            targetZoom = Input.mouseScrollDelta.y * _zoomSpeed;
        }

        var tooHigh = _camera.localPosition.y <= _zoomHeightLimits.x && targetZoom > 0;
        var tooLow = _camera.localPosition.y >= _zoomHeightLimits.y && targetZoom < 0;
        if (tooHigh || tooLow) {
            _deltaZoom = 0;
        }
        else {
            _deltaZoom = Mathf.Lerp(_deltaZoom, targetZoom, _zoomFriction * Time.deltaTime);
        }

        var oldPos = _camera.localPosition;
        _camera.position += _camera.forward * _deltaZoom;
        
        if (oldPos.y < _zoomHeightLimits.y && _camera.localPosition.y > _zoomHeightLimits.y) {
            _camera.localPosition = oldPos;
        }

        if (oldPos.y > _zoomHeightLimits.x && _camera.localPosition.y < _zoomHeightLimits.x) {
            _camera.localPosition = oldPos;
        }
    }

    private void Tilt()
    {
        var progress = Mathf.InverseLerp(_zoomHeightLimits.x, _zoomHeightLimits.y, _camera.localPosition.y);
        var x = Mathf.Lerp(_zoomAngleLimits.x, _zoomAngleLimits.y, progress);

        _camera.localEulerAngles = Vector3.Lerp(_camera.localEulerAngles, Vector3.right * x, _zoomTiltFriction * Time.deltaTime);
    }

    private void Rot()
    {
        var targetRot = 0f;
        if (Input.GetMouseButton(0)) {
            var deltaX = Input.GetAxis("Mouse X");
            targetRot = deltaX * _rotationSpeed;
            if (Mathf.Abs(deltaX) > 0.2f) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        _deltaRot = Mathf.Lerp(_deltaRot, targetRot, _rotationFriction * Time.deltaTime);

        transform.localEulerAngles += Vector3.up * _deltaRot;

        if (Input.GetMouseButtonUp(0)) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}

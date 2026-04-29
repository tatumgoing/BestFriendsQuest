using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParkFreecam : MonoBehaviour
{
    [SerializeField] private Transform _leashCenter;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float _fovSpeed;
    [SerializeField] private float _fovLerpFactor = 10;
    [SerializeField] private Vector2 _fovLimits;
    [SerializeField] private float _maxDist;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveLerpFactor;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _rotLerpFactor;

    private Vector3 _moveDelta;
    private float _rotDelta;
    private float _fovDelta;
    private bool _hidingMouse;

    private void Update()
    {
        HandleMovement();

        if (_hidingMouse) {
            HandleRot();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            var scroll = Input.mouseScrollDelta.y;
            var targetFovDelta = scroll * _fovSpeed * Time.deltaTime;
            _fovDelta = Mathf.Lerp(_fovDelta, targetFovDelta, _fovLerpFactor * Time.deltaTime);

            var newFov = _cam.m_Lens.FieldOfView + _fovDelta;
            newFov = Mathf.Clamp(newFov, _fovLimits.x, _fovLimits.y);
            _cam.m_Lens.FieldOfView = newFov;
        }
        else {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) _hidingMouse = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _hidingMouse = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnEnable()
    {
        _hidingMouse = true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandleRot()
    {
        var mouseDelta = Input.GetAxis("Mouse X");
        var targetRot = mouseDelta * _rotSpeed * Time.deltaTime;
        _rotDelta = Mathf.Lerp(_rotDelta, targetRot, _rotLerpFactor * Time.deltaTime);
        transform.localEulerAngles += Vector3.up * _rotDelta;
    }

    private void HandleMovement()
    {
        var forward = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0;
        var backward = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0;
        var right = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;
        var left = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0;

        var dir = new Vector3(forward - backward, 0, right - left).normalized;
        var moveDir = transform.forward * dir.x + transform.right * dir.z;

        var oldMoveDelta = _moveDelta;
        var targetMoveDelta = moveDir * _moveSpeed * Time.deltaTime;
        _moveDelta = Vector3.Lerp(_moveDelta, targetMoveDelta, _moveLerpFactor * Time.deltaTime);

        var newPos = transform.position + _moveDelta;

        var newDist = Vector3.Distance(_leashCenter.position, newPos);
        var currentDist = Vector3.Distance(_leashCenter.position, transform.position);

        if (newDist < _maxDist || newDist < currentDist) transform.position = newPos;
        else {
            _moveDelta = Vector3.Lerp(oldMoveDelta, Vector3.zero, _moveLerpFactor * Time.deltaTime);
            transform.position += _moveDelta;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_leashCenter.position, _maxDist);
    }
}

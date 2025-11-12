using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _lerpFactor = 10;
    [SerializeField] private float _headZoom;
    [SerializeField] private float _bodyZoom;
    [SerializeField] private float _headYOffset = 0;
    [SerializeField] private float _bodyYOffset = -2;
    [SerializeField] private float _buttonZoomAmount = 2;
    [SerializeField] private Vector2 _minMaxZoomHead;
    [SerializeField] private Vector2 _minMaxZoomBody;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private Transform _character;

    [Header("Free Look")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _freelookParent;
    [SerializeField] private float _freelookLerpFactor = 10;
    [SerializeField] private float _freelookZoomLerpFactor = 8;
    [SerializeField] private float _freelookZoomSpeed = 5;
    [SerializeField] private Vector2 _freelookZoomLimits = new Vector2(0.1f, 2);
    [SerializeField] private bool _freeLook;
    [SerializeField] private Vector2 _freeLookSpeed = Vector2.one;
    [SerializeField] private Vector3 _freelookCamEuler;

    private float _freeLookZoomTarget = 10;
    private float _currentFreelookZoom = 2;
    private Quaternion _freelookTargetRot;
    private float _freeLookPitch;
    private float _freeLookYaw;
    private Vector3 _originalCamEuler;
    private Vector3 _freeLookOffset;
    private bool _body;
    private Vector3 _targetPosition;

    [HideInInspector] public bool UsingRotateControls;

    private Vector2 _minMaxZoom => _body ? _minMaxZoomBody : _minMaxZoomHead;
    private float _currentBaseZoom => _body ? _bodyZoom : _headZoom;
    private float _currentDist => Vector3.Distance(_targetPosition, _character.position);
    private Vector3 _centerPosition => _character.position + (_getCurrentDir() * _currentBaseZoom);
    private Vector3 _minPosition => _centerPosition + _getCurrentDir() * _minMaxZoom.x;
    private Vector3 _maxPosition => _centerPosition + _getCurrentDir() * _minMaxZoom.y;
    private float _minDist => Vector3.Distance(_character.position, _minPosition);
    private float _maxDist => Vector3.Distance(_character.position, _maxPosition);

    private void Start()
    {
        _freelookTargetRot = _freelookParent.rotation;
        _originalCamEuler = _camera.localEulerAngles;
        _freeLookOffset = _freelookParent.InverseTransformPoint(transform.position);
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_freeLook) {
            if (!EventSystem.current.IsPointerOverGameObject()) { 
                if (Input.GetMouseButton(1)) {
                    FreeLook();
                }
                else if (Cursor.lockState == CursorLockMode.Locked && !UsingRotateControls) {
                    Cursor.lockState = CursorLockMode.Confined;
                }

                var scrollDelta = -Input.mouseScrollDelta.y;
                _freeLookZoomTarget += scrollDelta;
                _freeLookZoomTarget = Mathf.Clamp(_freeLookZoomTarget, _freelookZoomLimits.x, _freelookZoomLimits.y);
            }

            _currentFreelookZoom = Mathf.Lerp(_currentFreelookZoom, _freeLookZoomTarget, _freelookZoomLerpFactor * Time.deltaTime);

            _freelookParent.rotation = Quaternion.Lerp(_freelookParent.rotation, _freelookTargetRot, _lerpFactor * Time.deltaTime);

            transform.position = _freelookParent.TransformPoint(_freeLookOffset);
            var dir = (transform.position - _freelookParent.position).normalized;
            transform.position = _freelookParent.position + (dir * _currentFreelookZoom);


            transform.LookAt(_freelookParent.position);

            return;
        }
        else _camera.localEulerAngles = _originalCamEuler;

        if (!EventSystem.current.IsPointerOverGameObject()) Scroll();

        _targetPosition.y = _character.position.y + (_body ? _bodyYOffset : _headYOffset);

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _freelookLerpFactor * Time.deltaTime);
    }

    [ButtonMethod]
    public void SwitchToFreeLook() {
        _freeLook = true;
        FreeLook();
    }

    public void SwtichToStatic()
    {
        _freeLook = false;
        _camera.localEulerAngles = _originalCamEuler;
        transform.localEulerAngles = new Vector3(0, -25, 0);
        //transform.localEulerAngles = Vector3.zero;
    }
       
    private void FreeLook()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _camera.localEulerAngles = _freelookCamEuler;

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        _freeLookPitch -= mouseY * _freeLookSpeed.y;
        _freeLookYaw += mouseX * _freeLookSpeed.x;

        _freeLookPitch = Mathf.Clamp(_freeLookPitch, 0, 80f);
        _freelookTargetRot = Quaternion.Euler(_freeLookPitch, _freeLookYaw, 0);
    }

    private Vector3 _getCurrentDir()
    {
        var dir = (_targetPosition - _character.position).normalized;
        dir.y = 0;
        return dir;
    }

    public void ZoomOut()
    {
        var zoomAmount = Mathf.Min(_buttonZoomAmount, (_currentBaseZoom + _minMaxZoom.y) - _currentDist);
        _targetPosition += _getCurrentDir() * zoomAmount;
    }

    public void ZoomIn()
    {
        var zoomAmount = Mathf.Min(_buttonZoomAmount, _currentDist - (_currentBaseZoom + _minMaxZoom.x));
        _targetPosition += _getCurrentDir() * -zoomAmount;
    }

    public void ResetZoom(bool instant = false)
    {
        _targetPosition = _character.position + (_getCurrentDir() * _currentBaseZoom);
        if (instant) {
            _targetPosition.y = _character.position.y + (_body ? _bodyYOffset : _headYOffset);
            transform.position = _targetPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(_character.position, _getCurrentDir());

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_centerPosition, 0.5f);
        Gizmos.DrawLine(_minPosition, _maxPosition);
    }

    [ButtonMethod]
    public void SetHeadDistance()
    {
        SetDist(_headZoom, _headYOffset);
        _body = false;
    }

    [ButtonMethod]
    public void SetBodyDistance()
    {
        SetDist(_bodyZoom, _bodyYOffset);
        _body = true;
    }

    public void SetDist(float dist, float yOffset = 0)
    {
        var dir = (_character.position - _targetPosition).normalized;
        dir.y = 0;
        _targetPosition = _character.position;
        _targetPosition -= dir * dist;
        _targetPosition += Vector3.up * yOffset;
    }

    private void Scroll()
    {
        float scrollDelta = -Input.mouseScrollDelta.y;

        if (EventSystem.current.IsPointerOverGameObject() || !Application.isFocused) scrollDelta = 0;

        var mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height) scrollDelta = 0;

        var zoomAmount = scrollDelta * _zoomSpeed * Time.deltaTime * 10;
        
        var posDelta = _getCurrentDir() * zoomAmount;

        if (Vector3.Distance(_character.position, _targetPosition + posDelta) < _minDist) _targetPosition = _minPosition;
        else if (Vector3.Distance(_character.position, _targetPosition + posDelta) > _maxDist) _targetPosition = _maxPosition;
        else _targetPosition += posDelta;
    }
}

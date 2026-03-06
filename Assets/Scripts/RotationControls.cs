using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Axis { NONE, X, Y, Z }

public class RotationControls : MonoBehaviour
{
    [SerializeField] private Transform _xRing;
    [SerializeField] private Transform _yRing;
    [SerializeField] private Transform _zRing;

    private Vector3 _alphaRange = new Vector3(0.45f, 0.8f, 1f);
    [SerializeField] private LayerMask _hoverLayers;
    private float _rotationSpeed = 2.5f;
    private AddonsUIHelper _uiController;

    private MovableAddon _controller;
    private bool _rotating;
    private Axis _currentAxis = Axis.NONE;
    private Vector3 _rotAxis;
    private Quaternion _startRot;
    private Quaternion _mirrorStartRot;
    private float _rotAngle = 0;

    private Transform _mirror => _controller.Mirror.transform.GetChild(0);

    private void Start()
    {
        _uiController = FindObjectOfType<AddonsUIHelper>();
        _controller = GetComponentInParent<MovableAddon>();
        SetAllAlphas(_alphaRange.x);
    }

    private void Update()
    {
        if (!_controller.Selected || !_uiController.Rotating) return;

        if (_rotating) {
            Rotate();
            return;
        }

        var didHover = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hoverInfo, 1000, _hoverLayers);
        if (!didHover) return;

        _currentAxis = Axis.NONE;
        if (Input.GetMouseButton(0)) StartRotate(hoverInfo);
        else {
            SetAllAlphas(_alphaRange.x);
            SetColorAlpha(hoverInfo.collider.transform, _alphaRange.y);
        }
    }

    private void StartRotate(RaycastHit hoverInfo)
    {
        void SetRotStart(Axis currentAxis, Vector3 rotAxis, Transform selectedRing)
        {
            _currentAxis = currentAxis;
            _rotAxis = rotAxis;
            SetColorAlpha(selectedRing, _alphaRange.z);

            FindObjectOfType<CameraController>().UsingRotateControls = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _startRot = transform.parent.rotation;
            _mirrorStartRot = _mirror.rotation;
            _rotAngle = 0;
            _rotating = true;
        }

        if (hoverInfo.collider.transform == _xRing) {
            SetRotStart(Axis.X, Vector3.right, _xRing);
        }

        if (hoverInfo.collider.transform == _yRing) {
            SetRotStart(Axis.Y, Vector3.up, _yRing);
        }

        if (hoverInfo.collider.transform == _zRing) {
            SetRotStart(Axis.Z, Vector3.forward, _zRing);
        }
    }

    private void Rotate()
    {
        _rotAngle -= Input.GetAxis("Mouse X") * _rotationSpeed;
        transform.parent.rotation = _startRot * Quaternion.AngleAxis(_rotAngle, _rotAxis);

        if (_currentAxis == Axis.X) {
            _mirror.rotation = _mirrorStartRot * Quaternion.AngleAxis(_rotAngle, _rotAxis);
        }
        else {
            _mirror.rotation = _mirrorStartRot * Quaternion.AngleAxis(-_rotAngle, _rotAxis);
        }

        if (!_mirror.gameObject.activeInHierarchy && _currentAxis == Axis.Z) {
            _mirror.rotation = _mirrorStartRot * Quaternion.AngleAxis(_rotAngle, _rotAxis);
        }

        if (Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SetAllAlphas(_alphaRange.x);
            FindObjectOfType<CameraController>().UsingRotateControls = false;
            _rotating = false;
            _currentAxis = Axis.NONE;
        }
        else Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetAllAlphas(float alpha)
    {
        SetColorAlpha(_xRing, _alphaRange.x);
        SetColorAlpha(_yRing, _alphaRange.x);
        SetColorAlpha(_zRing, _alphaRange.x);
    }

    private void SetColorAlpha(Transform obj, float alpha)
    {
        var rend = obj.GetComponent<MeshRenderer>();
        var col = rend.material.color;
        col.a = alpha;
        rend.sharedMaterial.color = col;
    }
}

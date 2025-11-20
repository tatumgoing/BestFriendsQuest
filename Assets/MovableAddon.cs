using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableAddon : MonoBehaviour
{
    public bool Selected;
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private LayerMask _hoverLayers;
    [SerializeField] private GameObject _rotationControls;

    private bool _dragging;
    [HideInInspector] private Vector3 TargetUp;
    private AddonsUIHelper _uiController;
    private MovableAddon _mirror;
    private HairPiece _controller;

    public Transform Mirror => _mirror.transform;
    
    private void Start()
    {
        _uiController = FindObjectOfType<AddonsUIHelper>();
        _controller = GetComponentInParent<HairPiece>();
    }

    public void Initialize(MovableAddon mirror)
    {
        _mirror = mirror; 
    }

    private void Update()
    {
        if (_controller.IsMirroredVersion) {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        _rotationControls.SetActive(_uiController.Rotating && Selected);
        if (!Selected) return;

        Quaternion targetLocalRot = Quaternion.FromToRotation(Vector3.up, TargetUp) * Quaternion.identity;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRot, 15 * Time.deltaTime);

        if (_rotationControls.activeInHierarchy) {
            _dragging = false;
            return;
        }
        else {
            //Quaternion targetLocalRot = Quaternion.FromToRotation(Vector3.up, TargetUp) * Quaternion.identity;
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRot, 15 * Time.deltaTime);
        }

        if (!_dragging) {
            var didHover = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hoverInfo, 1000, _hoverLayers);
            if (!didHover || hoverInfo.collider.GetComponentInParent<MovableAddon>() != this) return;

            if (Input.GetMouseButtonDown(0)) _dragging = true;
        }
        else {

            if (Input.GetMouseButtonUp(0)) _dragging = false;

            var didHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 1000, _hitLayers);
            if (!didHit) return;

            TargetUp = transform.parent.InverseTransformDirection(hitInfo.normal);
            transform.position = hitInfo.point;

            if (_mirror) {

                var mirrorUp = Vector3.Scale(transform.parent.InverseTransformDirection(hitInfo.normal), new Vector3(-1, 1, 1));
                _mirror.TargetUp = Quaternion.AngleAxis(180f, mirrorUp) * mirrorUp;

                Vector3 localPos = transform.parent.InverseTransformPoint(hitInfo.point);
                localPos.x = -localPos.x;
                _mirror.transform.position = transform.parent.TransformPoint(localPos);
            }
        }
    }
}

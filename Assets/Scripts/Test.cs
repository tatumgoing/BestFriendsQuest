using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private LayerMask _hoverLayers;

    private bool _dragging;
    private Vector3 _targetUp;

    private void Update()
    {
        if (!_dragging) {
            var didHover = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hoverInfo, 1000, _hoverLayers);
            if (!didHover || hoverInfo.collider.transform.parent != transform) return;

            if (Input.GetMouseButtonDown(0)) _dragging = true;
        }
        else {

            if (Input.GetMouseButtonUp(0)) _dragging = false;

            var didHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 1000, _hitLayers);

            if (!didHit) return;

            _targetUp = hitInfo.normal;

            transform.position = hitInfo.point;
        }

        transform.up = Vector3.Lerp(transform.up, _targetUp, 15 * Time.deltaTime);

    }
}

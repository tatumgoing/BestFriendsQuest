using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScale : MonoBehaviour
{
    [SerializeField] private Vector3 _scale;
    [SerializeField] private bool _lossy;
    [SerializeField] private bool _symetric;
    [SerializeField, ReadOnly] private Vector3 _lossyScale;

    void Update() => SetScale();
    private void LateUpdate() => SetScale();

    private void OnValidate()
    {
        if (_symetric) _scale.y = _scale.z = _scale.x;
    }

    private void SetScale()
    {
        if (_scale.x != Mathf.Infinity) {
            print("Setting scale of " + name + " to " + _scale);
            if (_lossy) transform.SetLossyScale(_scale);
            else transform.localScale = _scale;
        }

        _lossyScale = transform.lossyScale;
    }
}

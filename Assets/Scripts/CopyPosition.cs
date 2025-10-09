using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private Transform _lead;
    [SerializeField] private bool _alsoRotation;
    [SerializeField] private bool _storeInitial;

    private Vector3 _offset = Vector3.zero;

    private void Start()
    {
        if (_storeInitial) {
            _offset = _lead.InverseTransformPoint(transform.position);
        }
    }

    void Update()
    {
        transform.position = _lead.TransformPoint(_offset);
        if (_alsoRotation) transform.rotation = _lead.rotation;
    }
}

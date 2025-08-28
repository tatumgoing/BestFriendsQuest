using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScale : MonoBehaviour
{
    [SerializeField] private Vector3 _scale;
    [SerializeField] private bool _lossy;
    [SerializeField, ReadOnly] private Vector3 _lossyScale;

    void Update()
    {
        if (_lossy) transform.SetLossyScale(_scale);
        else transform.localScale = _scale;

        _lossyScale = transform.lossyScale;
    }
}

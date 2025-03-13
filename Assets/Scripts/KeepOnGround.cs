using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnGround : MonoBehaviour
{
    [SerializeField] private Transform _detector;
    [SerializeField] private float _targetY;

    private void Update()
    {
        PutOnGround();
    }

    [ButtonMethod]
    private void PutOnGround()
    {
        if (Mathf.Abs(_detector.position.y - _targetY) < 0.01f) return;

        float myOffset = transform.position.y - _detector.position.y;
        var pos = transform.position;
        pos.y = _targetY + myOffset;
        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        var pos = transform.position;
        pos.y = _targetY;
        Gizmos.DrawWireSphere(pos, 1f);
    }
}

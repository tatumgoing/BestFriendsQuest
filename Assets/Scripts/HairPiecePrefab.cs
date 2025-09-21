using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class HairPiecePrefab : MonoBehaviour
{
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private Transform _rotPointStart;
    [SerializeField] private Transform _rotPointEnd;
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _hair;

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * 3));

        Gizmos.color = Color.green;
        if (_rotPointEnd && _rotPointStart) Gizmos.DrawLine(_rotPointStart.position, _rotPointEnd.position);

        Gizmos.color = Color.blue;
        if (_origin) Gizmos.DrawSphere(_origin.position, 0.1f);

        Gizmos.color = Color.magenta;
        if (_hair) Gizmos.DrawLine(_hair.position, _hair.position + _hair.up * 2);

    }


}

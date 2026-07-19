using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingChildren : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceLimit;

    private void Update()
    {
        foreach (Transform child in transform)
        {
            child.position += _direction.normalized * _speed * Time.deltaTime;
            if (child.localPosition.x < 0 && Vector3.Distance(transform.position, child.position) > _distanceLimit)
            {
                child.position += _direction * _distanceLimit * -1;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + _direction.normalized * _distanceLimit);
    }
}

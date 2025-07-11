using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _randomStart;
    [SerializeField] private bool _randomDir;

    private void Start()
    {
        if (_randomStart) transform.localEulerAngles = Vector3.up * Random.Range(0, 360);
        if (_randomDir) _speed *= Random.Range(-1, 1);
    }

    private void Update()
    {
        transform.localEulerAngles += Vector3.forward * _speed * Time.deltaTime;
    }
}

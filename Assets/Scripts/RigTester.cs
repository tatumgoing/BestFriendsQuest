using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScaleData
{
    [Range(0, 1)] public float Input = 0.5f;
    [SerializeField] private Vector3 _min;
    [SerializeField] private Vector3 _max;

    public Vector3 Current => Vector3.Lerp(_min, _max, Input);
}

public class RigTester : MonoBehaviour
{
    [SerializeField] private Transform _testBone;
    [SerializeField] private ScaleData _independentData;
    [SerializeField] private ScaleData _parentingData;


    private void Update()
    {
        _testBone.localScale = Vector3.Scale(_independentData.Current, _parentingData.Current);
        foreach (Transform child in _testBone) {
            var scale = Vector3.one;
            scale.x /= _independentData.Current.x;
            scale.y /= _independentData.Current.y;
            scale.z /= _independentData.Current.z;
            child.localScale = scale;
        }
    }
}

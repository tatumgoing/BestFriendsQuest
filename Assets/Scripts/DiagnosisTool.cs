using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosisTool : MonoBehaviour
{
    [SerializeField] private bool _printOnEnable;
    [SerializeField] private bool _printOnDisable;

    private void OnEnable()
    {
        if (_printOnEnable) print(gameObject.name + " enabled");
    }

    private void OnDisable()
    {
        if (_printOnDisable) print(gameObject.name + " disabled");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialColor : MonoBehaviour
{
    [SerializeField] private int _materialIndex;
    [SerializeField] private Renderer [] _renderers;
    [SerializeField] private bool _printChanges;

    public void SetColor(Color col)
    {
        if (_printChanges) print("color changed");

        if (!_renderers[0]) _renderers[0] = GetComponent<Renderer>();
        foreach(var renderer in _renderers)
            renderer.materials[_materialIndex].color = col;
    }
}

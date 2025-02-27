using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SetMaterialColor : MonoBehaviour
{
    [SerializeField] private int _materialIndex;
    private MeshRenderer _renderer;

    public void SetColor(Color col)
    {
        if (!_renderer) _renderer = GetComponent<MeshRenderer>();
        _renderer.materials[_materialIndex].color = col;
    }
}

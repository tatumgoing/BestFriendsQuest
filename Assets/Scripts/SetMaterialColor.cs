using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetMaterialColor : MonoBehaviour
{
    [SerializeField] private int _materialIndex;
    [SerializeField] private Renderer _renderer;

    public void SetColor(Color col)
    {
        if (!_renderer) _renderer = GetComponent<Renderer>();
        _renderer.materials[_materialIndex].color = col;
    }
}

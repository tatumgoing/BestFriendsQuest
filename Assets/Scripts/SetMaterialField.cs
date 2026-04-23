using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialField : MonoBehaviour
{
    [SerializeField] private int _materialIndex;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private string _fieldName;

    public void SetColor(Color color)
    {
        if (_materialIndex != 0) {
            _renderer.materials[_materialIndex].SetColor(_fieldName, color);
        }
        else _renderer.material.SetColor(_fieldName, color);
    }
}

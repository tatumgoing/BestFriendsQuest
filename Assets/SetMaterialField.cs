using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialField : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private string _fieldName;

    public void SetColor(Color color)
    {
        _renderer.material.SetColor(_fieldName, color);
    }
}

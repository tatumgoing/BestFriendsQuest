using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class SetHeightOnClothingShader : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottom;
    [SerializeField, ReadOnly] private float _height;

    private void Update()
    {
        if (!_top || !_bottom) return;

        _height = _top.position.y - _bottom.position.y;

        foreach (var r in _renderers) {
            if (Application.isPlaying) {
                r.material.SetFloat("_height", _height);
            }
            else {
                r.sharedMaterial.SetFloat("_height", _height);
            }
        }
        
    }
}

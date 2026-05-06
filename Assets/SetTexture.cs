using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SetTexture : MonoBehaviour
{
    [SerializeField] private int _materialIndex;
    private SkinnedMeshRenderer _renderer;

    public void Set(Texture texture)
    {
        if (!_renderer) _renderer = GetComponent<SkinnedMeshRenderer>();

        var mats = _renderer.materials;                  
        var mat = Instantiate(mats[_materialIndex]);     

        mat.SetTexture("_BaseMap", texture);

        mats[_materialIndex] = mat;                      
        _renderer.materials = mats;

        print("texutre changed. new texutre: " + texture.name);
    }
}

using MyBox;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class MainHairPrefab : MonoBehaviour
{
    [SerializeField, DisplayInspector] private FeatureSOData _data;

    [ButtonMethod]
    public void Save()
    {
        _data.MainHairLocalPosition = transform.localPosition;
        _data.MainHairLocalRotation = transform.localRotation;
        _data.MainHairLocalScale = transform.localScale;

    #if UNITY_EDITOR
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
    #endif

    }

    [ButtonMethod]
    public void Load()
    {
        transform.localPosition = _data.MainHairLocalPosition;
        transform.localRotation = _data.MainHairLocalRotation;
        transform.localScale = _data.MainHairLocalScale;
    }

    private void Update()
    {
        Load();
    }
}

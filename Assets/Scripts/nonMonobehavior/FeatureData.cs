using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


[System.Serializable]
public class FeatureData
{
    [HideInInspector] public string Name;
    public FeatureType Type;
    [ConditionalField(nameof(Type), true, FeatureType.EAR)]public FeatureSubType SubType;
    public Sprite Icon;

    [ConditionalField(nameof(Type), false, FeatureType.FACE)] public Texture2D Texture;
    [ConditionalField(nameof(Type), false, FeatureType.FACE)] public Texture2D ColorMask;

    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public Mesh Mesh;

    [ConditionalField(nameof(Type), false, FeatureType.EAR)] public GameObject EarPrefab;
    [ConditionalField(nameof(Type), false, FeatureType.EAR)] public Vector2 AngleLimits;

    [ConditionalField(nameof(Type), true, FeatureType.HAIR)] public Vector2 HoriLimits;
    [ConditionalField(nameof(Type), true, FeatureType.HAIR)] public Vector2 VertLimits;
    public Vector2 SizeLimits;

    [Header("Defaults")]
    [SerializeField] private FeatureObjSettings _defaultSettings;
    public FeatureObjSettings DefaultSettings => _defaultSettings;

    [HideInInspector] public FeatureCategory Category;

    public FeatureData() { }
    public FeatureData(FeatureData o)
    {
        Name = o.Name;
        Type = o.Type;
        SubType = o.SubType;
        Icon = o.Icon;

        Texture = o.Texture;
        ColorMask = o.ColorMask;

        Mesh = o.Mesh;

        EarPrefab = o.EarPrefab;
        AngleLimits = o.AngleLimits;

        HoriLimits = o.HoriLimits;
        VertLimits = o.VertLimits;
        SizeLimits = o.SizeLimits;

        Category = o.Category;

        _defaultSettings = new FeatureObjSettings(o.DefaultSettings);
    }
}
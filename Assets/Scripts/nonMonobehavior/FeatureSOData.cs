using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public enum FeatureSubType { BROWS, EYES, NOSE, LIPS, MISC, BANGS, BIG, STRANDS, ALL }
public enum FeatureType { FACE, HAIR, EAR }
public enum MirrorType { LEFT, BOTH, RIGHT }

[CreateAssetMenu(fileName = "New Facial Feature", menuName = "Facial Feature")]
public class FeatureSOData : ScriptableObject
{
    [HideInInspector] public string Name;
    public FeatureType Type;
    [ConditionalField(nameof(Type), true, FeatureType.EAR)] public FeatureSubType SubType;
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
}

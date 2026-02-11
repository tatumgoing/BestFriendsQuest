using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public enum FeatureSubType { BROWS, EYES, NOSE, LIPS, MISC, BANGS, BIG, STRANDS, ALL, ADDONS }
public enum FeatureType { FACE, HAIR, EAR }
public enum MirrorType { LEFT, BOTH, RIGHT }

[CreateAssetMenu(fileName = "New Facial Feature", menuName = "Facial Feature")]
public class FeatureSOData : ScriptableObject
{
    [HideInInspector] public string Name;
    public FeatureType Type;
    [ConditionalField(nameof(Type), true, FeatureType.EAR)] public FeatureSubType SubType;
    public Sprite Icon;

    [Tooltip("Higher priority features are shown at the beginning of the list. Ties are sorted alphabetically.")]
    public int Priority;

    [ConditionalField(nameof(Type), false, FeatureType.FACE)] public Texture2D Texture;
    [ConditionalField(nameof(Type), false, FeatureType.FACE)] public Texture2D ColorMask;

    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public GameObject MainhairPrefab;

    [ConditionalField(nameof(Type), true, FeatureType.HAIR), SerializeField] private float _horiLimit;
    [ConditionalField(nameof(Type), false, FeatureType.EAR), SerializeField] private float _earOffset;
    [ConditionalField(nameof(Type), true, FeatureType.HAIR), SerializeField] private Vector2 _vertLimits;

    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public bool IsMainHair;
    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public Vector3 MainHairLocalPosition;
    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public Quaternion MainHairLocalRotation;
    [ConditionalField(nameof(Type), false, FeatureType.HAIR)] public Vector3 MainHairLocalScale;

    [ConditionalField(nameof(Type), false, FeatureType.EAR)] public GameObject EarPrefab;
    [ConditionalField(nameof(Type), false, FeatureType.EAR)] public Vector2 AngleLimits;

    public Vector2 SizeLimits;

    [Header("Simple Mode")]
    [ConditionalField(nameof(Type), true, FeatureType.HAIR), SerializeField] private float _simpleHoriLimit;
    [ConditionalField(nameof(Type), true, FeatureType.HAIR), SerializeField] private Vector2 _simpleVertLimits;

    [Header("Defaults")]
    [SerializeField] private FeatureObjSettings _defaultSettings;
    public FeatureObjSettings DefaultSettings => _defaultSettings;
    public Vector2 HoriLimits => GetHoriLimits();
    public Vector2 VertLimits => GameManager.i ? _vertLimits : _simpleVertLimits;

    private Vector2 GetHoriLimits()
    {
        var horiLimits = GameManager.i.Advanced ? _horiLimit : _simpleHoriLimit;


        if (Type == FeatureType.EAR) return  new Vector2(_earOffset - _horiLimit, _earOffset+ _horiLimit);
        else return new Vector2(-_horiLimit, _horiLimit);
    }
}

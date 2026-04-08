using Cinemachine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RigTester : MonoBehaviour
{
    [SerializeField] private bool _updateParamsOnValidate;
    [SerializeField] private Transform _boneParent;

    [Header("Height")]
    [SerializeField, Range(0, 1)] private float _height = 0.5f;

    [SerializeField] private List<BoneSettingsData> _heightBoneSettings;


    [Header("Weight")]
    [SerializeField, Range(0, 1)] private float _weight = 0.5f;

    [SerializeField] private List<BoneSettingsData> _weightBoneSettings;


    [Header("Torso")]
    [SerializeField, Range(0, 1)] private float _torso = 0.5f;

    [SerializeField] private List<BoneSettingsData> _torsoBoneSettings;


    [Header("Waist")]
    [SerializeField, Range(0, 1)] private float _waist = 0.5f;

    [SerializeField] private List<BoneSettingsData> _waistBoneSettings;


    [Header("Arms")]
    [SerializeField, Range(0, 1)] private float _arms = 0.5f;

    [SerializeField] private List<BoneSettingsData> _armsBoneSettings;


    [Header("Legs")]
    [SerializeField, Range(0, 1)] private float _legs = 0.5f;

    [SerializeField] private List<BoneSettingsData> _legsBoneSettings;

    private List<RigBoneCoordinator> _allBones = new List<RigBoneCoordinator>();

    private void OnEnable()
    {
        if (_allBones.Count == 0) Initialize();
        OnValidate();

        void InitializeSettings(ref List<BoneSettingsData> boneSettings, BoneSliderName sliderName)
        {
            foreach (var boneData in boneSettings) {
                foreach (var rigBone in _allBones) {
                    if (rigBone.Name == boneData.AffectedBone) rigBone.RegisterScaleData(boneData, sliderName);
                }
            }
        }

        InitializeSettings(ref _heightBoneSettings, BoneSliderName.HEIGHT);
        InitializeSettings(ref _weightBoneSettings, BoneSliderName.WEIGHT);
        InitializeSettings(ref _torsoBoneSettings, BoneSliderName.TORSO);
        InitializeSettings(ref _armsBoneSettings, BoneSliderName.ARMS);
        InitializeSettings(ref _waistBoneSettings, BoneSliderName.WAIST);
        InitializeSettings(ref _legsBoneSettings, BoneSliderName.LEGS);
    }

    private void Initialize()
    {
        _allBones = _boneParent.GetComponentsInChildren<RigBoneCoordinator>(true).ToList();
    }

    private void OnValidate()
    {
        foreach (var b in _heightBoneSettings) b.OnValidate();
        foreach (var b in _weightBoneSettings) b.OnValidate();
        foreach (var b in _torsoBoneSettings) b.OnValidate();
        foreach (var b in _armsBoneSettings) b.OnValidate();
        foreach (var b in _waistBoneSettings) b.OnValidate();
        foreach (var b in _legsBoneSettings) b.OnValidate();

        if (!Application.isPlaying) return;

        if (_updateParamsOnValidate) UpdateBoneParameters();

        foreach (var b in _allBones) {
            b.UpdateValue(BoneSliderName.HEIGHT, _height);
            b.UpdateValue(BoneSliderName.WEIGHT, _weight);
            b.UpdateValue(BoneSliderName.TORSO, _torso);
            b.UpdateValue(BoneSliderName.ARMS, _arms);
            b.UpdateValue(BoneSliderName.WAIST, _waist);
            b.UpdateValue(BoneSliderName.LEGS, _legs);
        }
    }

    private void UpdateBoneParameters()
    {
        void UpdateSettings(ref List<BoneSettingsData> boneSettings, BoneSliderName sliderName)
        {
            foreach (var boneData in boneSettings) {
                foreach (var rigBone in _allBones) {
                    if (rigBone.Name == boneData.AffectedBone) rigBone.RegisterScaleData(boneData, sliderName);
                }
            }
        }

        UpdateSettings(ref _heightBoneSettings, BoneSliderName.HEIGHT);
        UpdateSettings(ref _weightBoneSettings, BoneSliderName.WEIGHT);
        UpdateSettings(ref _torsoBoneSettings, BoneSliderName.TORSO);
        UpdateSettings(ref _armsBoneSettings, BoneSliderName.ARMS);
        UpdateSettings(ref _waistBoneSettings, BoneSliderName.WAIST);
        UpdateSettings(ref _legsBoneSettings, BoneSliderName.LEGS);
    }

    private void LateUpdate()
    {
        foreach (var b in _allBones) b.UpdatePositionAndScale();
    }
}

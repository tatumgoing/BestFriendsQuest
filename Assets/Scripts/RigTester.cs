using Cinemachine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BoneSettingsDataGroup
{
    [HideInInspector] public string DisplayName;
    [SerializeField] private BoneSliderName _category;
    [SerializeField] private List<BoneSettingsData> _boneSettings;

    public void OnValidate()
    {
        foreach (var b in _boneSettings) b.OnValidate();
        DisplayName = _category.ToString();
    }

    public void UpdateSettings(List<RigBoneCoordinator> allBones)
    {
        foreach (var boneData in _boneSettings) {
            foreach (var rigBone in allBones) {
                if (rigBone.Name == boneData.AffectedBone) rigBone.RegisterScaleData(boneData, _category);
            }
        }
    }
}

public class RigTester : MonoBehaviour
{
    [SerializeField] private List<BoneSettingsDataGroup> _allSettings;


    [SerializeField] private bool _updateParamsOnValidate;
    [SerializeField] private Transform _boneParent;

    [Header("Sliders")]
    [SerializeField, Range(0, 1)] private float _height = 0.5f;
    [SerializeField, Range(0, 1)] private float _weight = 0.5f;
    [SerializeField, Range(0, 1)] private float _torso = 0.5f;
    [SerializeField, Range(0, 1)] private float _waist = 0.5f;
    [SerializeField, Range(0, 1)] private float _arms = 0.5f;
    [SerializeField, Range(0, 1)] private float _legs = 0.5f;

    [Header("OLD")]
    [SerializeField] private List<BoneSettingsData> _heightBoneSettings;
    [SerializeField] private List<BoneSettingsData> _weightBoneSettings;
    [SerializeField] private List<BoneSettingsData> _legsBoneSettings;
    [SerializeField] private List<BoneSettingsData> _armsBoneSettings;
    [SerializeField] private List<BoneSettingsData> _waistBoneSettings;
    [SerializeField] private List<BoneSettingsData> _torsoBoneSettings;

    private List<RigBoneCoordinator> _allBones = new List<RigBoneCoordinator>();

    private void OnValidate()
    {
        foreach (var settingGroup in _allSettings) settingGroup.OnValidate();

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

    private void OnEnable()
    {
        if (_allBones.Count == 0) Initialize();
        OnValidate();
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

    private void UpdateBoneParameters()
    {
        foreach (var settingGroup in _allSettings) settingGroup.UpdateSettings(_allBones);
    }

    private void LateUpdate()
    {
        foreach (var b in _allBones) b.UpdatePositionAndScale();
    }
}

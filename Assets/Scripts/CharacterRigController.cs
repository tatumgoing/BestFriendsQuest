using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public enum BoneName { CORE, CHEST, SHOULDER, UPPER_ARM, FOREARM, HAND, NECK, HEAD, THIGH, SHIN, FOOT, TOES, BELLY, HEEL, PELVIS}

[System.Serializable]
public class BoneData
{
    [HideInInspector] public string DisplayName;
    [SearchableEnum] public BoneName Name;
    public Transform Bone;
    public Dictionary<BoneSliderName, Vector3> _normalScaleMults = new Dictionary<BoneSliderName, Vector3>();
    public Dictionary<BoneSliderName, Vector3> _independentScaleMults = new Dictionary<BoneSliderName, Vector3>();

    public void UpdateValues(BoneSliderName sliderName, Vector3 scale, bool independent)
    {
        if (independent) _independentScaleMults[sliderName] = scale;
        else _normalScaleMults[sliderName] = scale;
    }

    public Vector3 CurrentNormal()
    {
        var normal = Vector3.one;
        foreach (var mult in _normalScaleMults) normal.Scale(mult.Value);
        return normal;
    }

    public Vector3 CurrentIndependent()
    {
        var independent = Vector3.one;
        foreach (var mult in _independentScaleMults) independent.Scale(mult.Value);
        return independent;
    }
}

public class CharacterRigController : MonoBehaviour
{
    [Header("NEW")]
    [SerializeField] private List<BoneSettingsDataGroup> _allSettings;
    [SerializeField] private bool _updateParamsOnValidate;
    [SerializeField] private Transform _boneParent;

    private List<RigBoneCoordinator> _allBones = new List<RigBoneCoordinator>();

    [Header("OLD")]
    [SerializeField] private List<BoneData> _bones;
    [SerializeField] private Transform _rootBone;
    [SerializeField] float _localScaleMultiplier = 1f;

    private bool _initialized;

    private void OnValidate()
    {
        foreach (var data in _bones) {
            data.DisplayName = data.Name.ToString();
            if (data.Bone) data.DisplayName += ": " + data.Bone.gameObject.name;
        }

        //NEW:
        foreach (var settingGroup in _allSettings) settingGroup.OnValidate();
        if (!Application.isPlaying) return;

        if (_updateParamsOnValidate) UpdateBoneParameters();

        /*foreach (var b in _allBones) {
            b.UpdateValue(BoneSliderName.HEIGHT, _height);
            b.UpdateValue(BoneSliderName.WEIGHT, _weight);
            b.UpdateValue(BoneSliderName.TORSO, _torso);
            b.UpdateValue(BoneSliderName.ARMS, _arms);
            b.UpdateValue(BoneSliderName.WAIST, _waist);
            b.UpdateValue(BoneSliderName.LEGS, _legs);
        }*/
    }

    private void OnEnable()
    {
        print(gameObject.name + " was enabled");

        Initialize();

        OnValidate();
    }

    private void Initialize()
    {
        if (_initialized || !_boneParent) return;

        if (_allBones.Count == 0) {
            _allBones = _boneParent.GetComponentsInChildren<RigBoneCoordinator>(true).ToList();
        }
        _initialized = true;
    }

    private void Start()
    {
        foreach (var b in _allBones) {
            b.UpdateValue(BoneSliderName.HEIGHT, 0.5f);
            b.UpdateValue(BoneSliderName.WEIGHT, 0.5f);
            b.UpdateValue(BoneSliderName.TORSO, 0.5f);
            b.UpdateValue(BoneSliderName.ARMS, 0.5f);
            b.UpdateValue(BoneSliderName.WAIST, 0.5f);
            b.UpdateValue(BoneSliderName.LEGS, 0.5f);
        }
    }

    private void OnDisable()
    {
        print(gameObject.name + " was disabled");
    }

    public void SetValue(float value, BoneSliderName slider)
    {
        if (!_initialized) Initialize();

        print("Setting value" + slider + " to " + value);
        foreach (var b in _allBones) {
            b.UpdateValue(slider, value);
        }
    }

    private void UpdateBoneParameters()
    {
        foreach (var settingGroup in _allSettings) settingGroup.UpdateSettings(_allBones);
    }

    private void LateUpdate()
    {
        foreach (var b in _allBones) b.UpdatePositionAndScale();
    }


    //NEW ^

    //OLD:

    public string GetSaveString()
    {
        return "";
    }

    public void ModifyBone(BoneName name, BoneSliderName sliderName, Vector3 localScale, bool independentScale)
    {
        var selected = _bones.Where(x => x.Name == name).ToList();
        foreach (var b in selected) b.UpdateValues(sliderName, localScale, independentScale);

        ScaleModel();
    }

    private void ScaleModel()
    {
        _rootBone.localScale = Vector3.one * _localScaleMultiplier;
        ScaleBoneRecursive(_rootBone);
    }

    private void ScaleBoneRecursive(Transform current)
    {
        BoneData foundData = null;
        var independent = Vector3.one;
        foreach (var b in _bones) {
            if (b.Bone != current) continue;
            
            foundData = b;
            break;
        }

        if (foundData != null) {
            independent = ScaleBoneWithData(foundData);
        }

        foreach (Transform child in current) {
            var scale = Vector3.one;
            scale.x /= independent.x;
            scale.y /= independent.y;
            scale.z /= independent.z;
            child.localScale = scale;

            ScaleBoneRecursive(child);
        }
    }

    private Vector3 ScaleBoneWithData(BoneData data)
    {
        var independant = data.CurrentIndependent();
        var normal = data.CurrentNormal();
        var newScale = data.Bone.localScale;
        newScale.Scale(normal);
        newScale.Scale(independant);

        //print("setting " + data.Bone.gameObject.name + " scale: " + data.Bone.localScale + " * " + normal + " * " + independant + " = " + newScale);

        data.Bone.localScale = newScale;

        return independant;        
    }
}

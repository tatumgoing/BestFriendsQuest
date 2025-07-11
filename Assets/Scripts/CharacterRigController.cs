using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public enum BoneName { CORE, CHEST, SHOULDER, UPPER_ARM, FOREARM, HAND, NECK, HEAD, THIGH, SHIN, FOOT, TOES}

[System.Serializable]
public class BoneData
{
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
    [SerializeField] private List<BoneData> _bones;
    [SerializeField] private Transform _rootBone;

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
        _rootBone.localScale = Vector3.one * 100;
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

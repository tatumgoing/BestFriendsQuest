using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BoneName { CORE, CHEST, SHOULDER, UPPER_ARM, FOREARM, HAND, NECK, HEAD, THIGH, SHIN, FOOT, TOES}

[System.Serializable]
public class  BoneData
{
    [SearchableEnum] public BoneName Name;
    [SerializeField] private Transform _guideBone; //scaling affects this bone and all children
    [SerializeField] private Transform _bone; //scaling only affects this bone

    public void SetScale(Vector3 localScale, bool guide)
    {
        var selected = guide ? _guideBone : _bone;
        var scale = selected.localScale;

        if (localScale.x > 0) scale.x = localScale.x;
        if (localScale.y > 0) scale.y = localScale.y;
        if (localScale.z > 0) scale.z = localScale.z;

        selected.localScale = scale;
    }
}

[System.Serializable]
public class BoneSliderData
{
    [SearchableEnum] public BoneName Bone;
    [SerializeField] private bool _horizontal;
    [SerializeField, ConditionalField(nameof(_horizontal))] private Vector2 _horizontalLimits;
    [SerializeField] private bool _vertical;
    [SerializeField, ConditionalField(nameof(_vertical))] private Vector2 _verticalLimits;
    public bool UseGuideBone;

    public Vector3 GetScale(float t)
    {
        var scale = Vector3.one * -1;
        if (_horizontal) {
            var _xz = Mathf.Lerp(_horizontalLimits.x, _horizontalLimits.y, t);
            scale.x = scale.z = _xz;
        }
        if (_vertical) {
            scale.y = Mathf.Lerp(_verticalLimits.x, _verticalLimits.y, t);
        }
        return scale;
    }
}

public class CharacterRigController : MonoBehaviour
{
    [SerializeField] private List<BoneData> _bones;

    public void ModifyBone(BoneName name, Vector3 localScale, bool guideBone)
    {
        var selected = _bones.Where(x => x.Name == name).ToList();
        foreach (var b in selected) b.SetScale(localScale, guideBone);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCustomizer : MonoBehaviour
{
    [SerializeField] private List<BoneSliderData> _heightSliderBones = new List<BoneSliderData>();
    [SerializeField] private List<BoneSliderData> _weightSliderBones = new List<BoneSliderData>();
    [SerializeField] private CharacterRigController _rigController;

    public void MoveHeightSlider(float value) => AffectRig(_heightSliderBones, value);
    public void MoveWeightSlider(float value) => AffectRig(_weightSliderBones, value);

    private void AffectRig(List<BoneSliderData> data, float value)
    {
        foreach (var bone in data) _rigController.ModifyBone(bone.Bone, bone.GetScale(value), bone.UseGuideBone);
    }
}

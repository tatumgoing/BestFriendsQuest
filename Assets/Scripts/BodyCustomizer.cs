using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCustomizer : MonoBehaviour
{
    [SerializeField] private List<BoneSliderData> _heightSliderBones = new List<BoneSliderData>();
    [SerializeField] private List<BoneSliderData> _weightSliderBones = new List<BoneSliderData>();
    [SerializeField] private CharacterRigController _rigController;
    [SerializeField] private GameObject _advancedMenu;

    [Header("advanced")]
    [SerializeField] private List<BoneSliderData> _armsSliderBones = new List<BoneSliderData>();
    [SerializeField] private List<BoneSliderData> _torsoSliderBones = new List<BoneSliderData>();
    [SerializeField] private List<BoneSliderData> _waistSliderBones = new List<BoneSliderData>();
    [SerializeField] private List<BoneSliderData> _legsSliderBones = new List<BoneSliderData>();

    public void MoveHeightSlider(float value) => AffectRig(_heightSliderBones, value);
    public void MoveWeightSlider(float value) => AffectRig(_weightSliderBones, value);
    public void MoveArmsSlider(float value) => AffectRig(_armsSliderBones, value);
    public void MoveTorsoSlider(float value) => AffectRig(_torsoSliderBones, value);
    public void MoveWaistSlider(float value) => AffectRig(_waistSliderBones, value);
    public void MoveLegsSlider(float value) => AffectRig(_legsSliderBones, value);

    private void Start()
    {
        _advancedMenu.SetActive(false);
    }

    private void AffectRig(List<BoneSliderData> data, float value)
    {
        foreach (var bone in data) _rigController.ModifyBone(bone.Bone, bone.GetScale(value), bone.UseGuideBone);
    }
}

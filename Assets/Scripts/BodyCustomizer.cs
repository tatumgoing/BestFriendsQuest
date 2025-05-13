using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("References")]
    [SerializeField] List<Slider> _allSliders = new List<Slider>();
    [SerializeField] private CheckBox _advancedCheck;

    private const string seperator = "%";

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

    public void LoadFromString(string saveString)
    {
        var list = saveString.Split(seperator).ToList();
        var advanced = false;
        for (int i = 0; i < list.Count; i++) {
            var value = float.Parse(list[i]);
            _allSliders[i].value = value;
            if (i > 1 && Mathf.Abs(0.5f - value) > 0.01f) advanced = true;
        }

        if (advanced) _advancedCheck.ToggleOn();
    }

    public string GetSaveString()
    {
        var list = new List<string>();
        foreach (var s in _allSliders) list.Add(RoundToHundreths(s.value).ToString());
        return string.Join(seperator, list);
    }

    private float RoundToHundreths(float input)
    {
        return (Mathf.Round(input * 100)) / 100f;
    }

    private void AffectRig(List<BoneSliderData> data, float value)
    {
        foreach (var bone in data) _rigController.ModifyBone(bone.Bone, bone.GetScale(value), bone.UseGuideBone);
    }
}

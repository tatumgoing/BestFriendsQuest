using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public enum BoneSliderName { NONE, HEIGHT, WEIGHT, ARMS, TORSO, WAIST, LEGS}

[System.Serializable]
public class BoneSliderGroupData
{
    [HideInInspector] public string Name;
    [HideInInspector] public BoneSliderName Type;
    public List<BoneSliderData> Bones = new List<BoneSliderData>();

    public BoneSliderGroupData(BoneSliderName type)
    {
        Type = type;
    }

    public void OnValidate()
    {
        Name = Type.ToString();
        foreach (var b in Bones) b.OnValidate();
    }
}

[System.Serializable]
public class BoneSliderData
{
    [HideInInspector] public string DisplayName;
    [SearchableEnum] public BoneName Name;
    [SerializeField, Tooltip("the more more uniform or subtle these values are, the less distorted the model will be")] private Vector3 _minScale;
    [SerializeField, Tooltip("the more more uniform or subtle these values are, the less distorted the model will be")] private Vector3 _maxScale;
    [Tooltip("When checked, this bone will scale independent of its children")] public bool IndependentScale;
    [Tooltip("When checked, this bone will scale independent of its children"), SerializeField] private bool _uniformScale;

    public Vector3 GetCurrent(float t) => Vector3.Lerp(_minScale, _maxScale, t);

    public void OnValidate()
    {
        DisplayName = Name.ToString();

        if (_minScale.magnitude <= 0.001 && _maxScale.magnitude <= 0.001) {
            _minScale = Vector3.one;
            _maxScale = Vector3.one;
        }

        if (_uniformScale) {
            _minScale.y = _minScale.z = _minScale.x;
            _maxScale.y = _maxScale.z = _maxScale.x;
        }
        _minScale.z = _minScale.x;
        _maxScale.z = _maxScale.x;
    }
}

public class BodyCustomizer : MonoBehaviour
{
    [SerializeField] private List<BoneSliderGroupData> _sliderGroups = new List<BoneSliderGroupData>();

    [SerializeField] private CharacterRigController _rigController;
    [SerializeField] private GameObject _advancedMenu;

    [Header("References")]
    [SerializeField] List<Slider> _allSliders = new List<Slider>();
    [SerializeField] private CheckBox _advancedCheck;
    [SerializeField] private Animator _mainAnimator; 

    private const string seperator = "%";

    public void MoveHeightSlider(float value) => AffectRig(BoneSliderName.HEIGHT, value);
    public void MoveWeightSlider(float value) => AffectRig(BoneSliderName.WEIGHT, value);
    public void MoveArmsSlider(float value) => AffectRig(BoneSliderName.ARMS, value);
    public void MoveTorsoSlider(float value) => AffectRig(BoneSliderName.TORSO, value);
    public void MoveWaistSlider(float value) => AffectRig(BoneSliderName.WAIST, value);
    public void MoveLegsSlider(float value) => AffectRig(BoneSliderName.LEGS, value);

    private void OnValidate()
    {
        var sliderGroupNames = Utils.EnumToList<BoneSliderName>();
        sliderGroupNames.RemoveAt(0); 

        for (int i = 0; i < sliderGroupNames.Count; i++) {
            if (i >= _sliderGroups.Count) _sliderGroups.Add(new BoneSliderGroupData(sliderGroupNames[i]));
            else _sliderGroups[i].Type = sliderGroupNames[i];

            _sliderGroups[i].OnValidate();
        }

        while(_sliderGroups.Count > sliderGroupNames.Count) _sliderGroups.RemoveAt(_sliderGroups.Count - 1);
    }

    private void OnEnable()
    {
        _mainAnimator.SetTrigger("Right");
    }

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

    private void AffectRig(BoneSliderName sliderGroupName, float value)
    {
        var data = _sliderGroups.Where(x => x.Type == sliderGroupName).FirstOrDefault();
        if (data == default) return;
        foreach (var bone in data.Bones) _rigController.ModifyBone(bone.Name, sliderGroupName, bone.GetCurrent(value), bone.IndependentScale); 
    }
}

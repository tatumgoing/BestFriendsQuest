using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMetaController : MonoBehaviour
{
    [ReadOnly] public string ID;

    [Header("Mode")]
    [SerializeField, Tooltip("Check yes in character creator scene, leave unchecked everywhere else")] private bool _isCharacterCreator = false;

    [SerializeField, ConditionalField(nameof(_isCharacterCreator))] private BodyCustomizer _bodyCustomizer;
    [SerializeField, ConditionalField(nameof(_isCharacterCreator))] private DataPanelController _dataPanel;
    [SerializeField, ConditionalField(nameof(_isCharacterCreator), inverse:true)] private FavoriteColorClothingInterface _clothingInterface;

    [Header("Rig")]
    [SerializeField] private CharacterRigController _rigController;
    [SerializeField] private List<BoneSliderGroupData> _rigGroups = new List<BoneSliderGroupData>();

    [Header("Misc")]
    [SerializeField] private FaceFeatureController _face;
    [SerializeField] private HairController _hair;
    [SerializeField] private EarController _ears;
    [SerializeField] private Color _skinColor;
    [SerializeField] private SetMaterialColor _skin;

    [Header("Expressions")]
    [SerializeField, DisplayInspector] private ExpressionSetData _expressionSet;
    public bool Blinking;
    public bool Talking;

    [ReadOnly] public CharacterProfileData Data;

    private float _blinkCooldown = -Mathf.Infinity;
    private float _blinkDuratingCooldown = 0;
    private float _talkingCooldown = -Mathf.Infinity;
    private Expression _currentExpression;
    private bool _eyesClosed;
    private bool _mouthOpen;

    public Color SkinColor => _skinColor;
    [ButtonMethod] public void setNeutral() => SetExpression(Expression.NEUTRAL);
    [ButtonMethod] public void setHappy() => SetExpression(Expression.HAPPY);
    [ButtonMethod] public void setSurprised() => SetExpression(Expression.SURPRISED);
    [ButtonMethod] public void setAngry() => SetExpression(Expression.ANGRY);
    [ButtonMethod] public void setSad() => SetExpression(Expression.SAD);

    private void Start()
    {
        SetSkinColor(_skinColor);
        _currentExpression = Expression.NEUTRAL;
    }

    private void Update()
    {
        HandleBlink();
        HandleTalking();
    }

    public void MakeNewID()
    {
        ID = "";
        for (int i = 0; i < GameManager.idLength; i++) {
            ID += Random.Range(0, 10);
        }
    }

    private void HandleTalking()
    {
        _talkingCooldown -= Time.deltaTime;

        if (!Talking) {
            if (_mouthOpen) SetExpression(_currentExpression, _eyesClosed, false);
            return;
        }

        if (_talkingCooldown == -Mathf.Infinity) _talkingCooldown = _expressionSet.TalkingSpeed;

        if (_talkingCooldown <= 0) {
            _talkingCooldown = _expressionSet.TalkingSpeed;

            if (_mouthOpen) {
                SetExpression(_currentExpression, _eyesClosed, false);
            }
            else {
                SetExpression(_currentExpression, _eyesClosed, true);
            }
        }

    }

    private void HandleBlink()
    {
        _blinkDuratingCooldown -= Time.deltaTime;
        if (_blinkDuratingCooldown <= 0) _blinkCooldown -= Time.deltaTime;
        if (!Blinking) {
            if (_eyesClosed) SetExpression(_currentExpression, false, _mouthOpen);
            return;
        }
        
        if (_blinkCooldown == -Mathf.Infinity) _blinkCooldown = _expressionSet.GetBlinkCooldown();
        if (_blinkDuratingCooldown > 0) return;

        SetExpression(_currentExpression, false, _mouthOpen);

        if (_blinkCooldown < 0) {
            SetExpression(_currentExpression, true, _mouthOpen);
            _blinkDuratingCooldown = _expressionSet.BlinkDuration;
            _blinkCooldown = _expressionSet.GetBlinkCooldown();
        }        
    }

    public void SetExpression(Expression expression, bool eyesClosed, bool mouthOpen)
    {
        _eyesClosed = eyesClosed;
        _mouthOpen = mouthOpen;

        _currentExpression = expression;

        var expressionData = _expressionSet.GetExpressionData(expression);
        var secondary = eyesClosed ? _expressionSet.GetExpressionData(Expression.BLINKING) : null;
        var tertiary = mouthOpen ? _expressionSet.GetExpressionData(Expression.TALKING) : null;

        _face.SetExpression(expressionData, secondary, tertiary);
    }

    public void SetExpression(Expression expression)
    {
        _currentExpression = expression;

        var expressionData = _expressionSet.GetExpressionData(expression);
        if (expressionData != null) _face.SetExpression(expressionData);
    }

    public void LoadFromString(string input)
    {
        input = input.Replace("\n", "");

        ID = input[..GameManager.idLength];
        input = input.Substring(GameManager.idLength);

        var parts = input.Split('|');
        _face.LoadFromString(parts[0]);
        _hair.LoadFromString(parts[1]);
        _ears.LoadFromString(parts[2]);

        ColorUtility.TryParseHtmlString(parts[3], out _skinColor);
        SetSkinColor(_skinColor);

        Data = new CharacterProfileData();
        Data.FromString(parts[5]);

        if (_isCharacterCreator) {
            _bodyCustomizer.LoadFromString(parts[4]);
            _dataPanel.Load(Data);
        }
        else {
            LoadRigInGame(parts[4]);
            _clothingInterface.SetColor(Data.FavColor);
        }

        print("LOADED CHARACTER");
        gameObject.SetActive(true);
    }

    private void LoadRigInGame(string rigSaveString)
    {
        var loadedSliderValues = rigSaveString.Split('%').Select(x => float.Parse(x)).ToList();

        void AffectRig(BoneSliderName sliderGroupName, float value)
        {
            var data = _rigGroups.Where(x => x.Type == sliderGroupName).FirstOrDefault();
            if (data == default) return;
            foreach (var bone in data.Bones) _rigController.ModifyBone(bone.Name, sliderGroupName, bone.GetCurrent(value), bone.IndependentScale);
        }

        AffectRig(BoneSliderName.HEIGHT, loadedSliderValues[0]);
        AffectRig(BoneSliderName.WEIGHT, loadedSliderValues[1]);
        AffectRig(BoneSliderName.ARMS, loadedSliderValues[2]);
        AffectRig(BoneSliderName.TORSO, loadedSliderValues[3]);
        AffectRig(BoneSliderName.WAIST, loadedSliderValues[4]);
        AffectRig(BoneSliderName.LEGS, loadedSliderValues[5]);
    }

    public string GetSaveString()
    {
        var dataSaveString = Data.ToString().Replace("___", ID);

        var list = new List<string>
        {
            _face.GetSaveString(),
            _hair.GetSaveString(),
            _ears.GetSaveString(),
            _skinColor.ToHex(),
            _bodyCustomizer.GetSaveString(),
            dataSaveString,
        };

        return ID + string.Join("|", list);
    }

    public void SetSkinColor(Color color)
    {
        _skinColor = color;
        _skin.SetColor(_skinColor);
        _ears.SetColor(_skinColor);
    }

}

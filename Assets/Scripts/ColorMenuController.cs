using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class BasicColorData
{
    public string Name;
    public Color Color;
}

public class ColorMenuController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private UnityEvent<Color> _onChangeColor;

    [Header("Modes")]
    [SerializeField] private Animator _basicParent;
    [SerializeField] private Animator _advancedParent;

    [Header("Basic mode")]
    [SerializeField] private List<BasicColorData> _basicColors;
    [SerializeField] private Transform _basicGridParent;

    [Header("Advanced Mode")]
    [SerializeField] private RawImage _satValImg;
    [SerializeField] private RawImage _hueImg;
    [SerializeField] private Image _currentColorImg;
    [SerializeField] private Slider _hueSlider;
    [SerializeField] private HexColorInputField _hexInput;
    [SerializeField] private GameObject _hexSectionParent;
    [SerializeField] private FollowMouseInRectBounds _advancedSelector;

    private List<BasicColorOption> _basicOptions = new List<BasicColorOption>();
    private float _hue = 0.5f;
    private float _sat = 0.5f;
    private float _val = 0.5f;
    private bool _inputingHex;
    private Texture2D _hueTex;
    private Texture2D _satValTex;
    private bool _invoke;
    private bool _advanced;
    private Color _basicColor;

    private float _currentHue { get { return _hue; } set { _hue = Mathf.Abs(value); } }
    private float _currentSat { get { return _sat; } set { _sat = Mathf.Abs(value); } }
    private float _currentVal { get { return _val; } set { _val = Mathf.Abs(value); } }
    private Color _currentColor => Color.HSVToRGB(_currentHue, _currentSat, _currentVal);

    private void OnEnable()
    {
        _advancedSelector.FollowMouse = _advanced;
    }

    private void Start()
    {
        _basicOptions = _basicGridParent.GetComponentsInChildren<BasicColorOption>().ToList();
        for (int i = 0; i < _basicOptions.Count; i++) {
            if (i >= _basicColors.Count) break;

            _basicOptions[i].Initialize(_basicColors[i], this);
        }

        _basicOptions[0].SelectButton();

        CreateTextures();
        _advancedSelector.enabled = false;
        _invoke = false;
        UpdateHue();
        _invoke = true;

        SetFromHexCode(_defaultColor.ToHex());
        UpdateCurrentColor();

        SetMode(false);
    }

    private void Update()
    {
        if (_advancedSelector.enabled) {
            if (Input.GetMouseButtonUp(0)) StopSelecting();
            else UpdateCurrentColor();
        } 
    }

    public void SelectBasicColor(Color color, BasicColorOption selected)
    {
        foreach (var o in _basicOptions) if (o != selected) o.Deselect();
        _basicColor = color;
        SetFromHexCode(color.ToHex());
    }

    public void SetMode(bool advanced)
    {
        if (advanced) {
            _basicParent.transform.SetAsFirstSibling();
            _advancedParent.gameObject.SetActive(true);
            _basicParent.SetTrigger("Exit");
        }
        else {
            _advancedParent.transform.SetAsFirstSibling();
            _basicParent.gameObject.SetActive(true);
            _advancedParent.SetTrigger("Exit");
            SetFromHexCode(_basicColor.ToHex());
        }

        _advancedSelector.FollowMouse = advanced;
        _hexSectionParent.SetActive(advanced);
        _advanced = advanced;

        _advancedSelector.GetComponent<Image>().enabled = advanced;

        UpdateCurrentColor();
    }

    public void SetFromHexCode(string hex)
    {
        _inputingHex = true;
        ColorUtility.TryParseHtmlString(hex, out var rgb);
        Color.RGBToHSV(rgb, out _hue, out _sat, out _val);
        _hueSlider.value = _currentHue;
        _advancedSelector.SetPosition(new Vector2(_currentSat, _currentVal));
        UpdateCurrentColor();
    }

    public void UpdateHue()
    {
        _currentHue = _hueSlider.value;
        UpdateSatVal();
        UpdateCurrentColor();
    }

    private void UpdateCurrentColor()
    {

        if (!_inputingHex) {
            bool updateSatVal = true;
            bool updateColor = true;

            var pos = _advancedSelector.GetNormalizedPositionFromCenter();
            if (pos.x > 1 || pos.x < 0 || pos.y > 1 || pos.y < 0) updateColor = false;

            if (updateColor && updateSatVal) {
                _currentSat = pos.x;
                _currentVal = pos.y;
            }

            _hexInput.UpdateText(_currentColor.ToHex().ToUpper());
        }

        _currentColorImg.color = _currentColor;
        if (_invoke) _onChangeColor.Invoke(_currentColor);
    }

    private void StopSelecting()
    {
        _advancedSelector.enabled = false;
    }

    public void StartSelecting()
    {
        _inputingHex = false;
        if (_advanced) _advancedSelector.enabled = true;
    }

    private void CreateTextures()
    {
        CreateHueImg();
        CreateSatValTex();
    }

    private void CreateSatValTex()
    {
        int height = 16;
        _satValTex = new Texture2D(height, height);
        _satValTex.wrapMode = TextureWrapMode.Clamp;
        _satValTex.name = "SatValTex";
    }

    private void CreateHueImg()
    {
        float height = 16;
        _hueTex = new Texture2D(1, (int)height);
        _hueTex.wrapMode = TextureWrapMode.Clamp;
        _hueTex.name = "HueText";

        for (int i = 0; i < height; i++) {
            var col = Color.HSVToRGB(i / height, 1, 1);
            _hueTex.SetPixel(0, i, col);
        }

        _hueTex.Apply();
        _hueImg.texture = _hueTex;
    }

    [ButtonMethod]
    private void UpdateSatVal()
    {
        if (_satValTex == null) CreateSatValTex();
        float height = _satValTex.height;
        for (int x = 0; x < _satValTex.width; x++) {
            for (int y = 0; y < _satValTex.height; y++) {
                var col = Color.HSVToRGB(_currentHue, x / height, y / height);
                _satValTex.SetPixel(x, y, col);
            }
        }
        _satValTex.Apply();
        _satValImg.texture = _satValTex;
    }
}

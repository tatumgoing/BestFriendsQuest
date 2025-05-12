using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorMenuController : MonoBehaviour
{
    [SerializeField] private RawImage _satValImg;
    [SerializeField] private RawImage _hueImg;
    [SerializeField] private Image _currentColorImg;
    [SerializeField] private Slider _hueSlider;
    [SerializeField] private HexColorInputField _hexInput;
    [SerializeField, Range(0, 1)] private float _hueValue = 0.5f;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private UnityEvent<Color> _onChangeColor;

    [Header("Modes")]
    [SerializeField] private float _gridResolution = 6;
    [SerializeField] private Transform _squaresParent;
    [SerializeField] private Material _unselectedSquare;
    [SerializeField] private Material _selectedSquare;
    [SerializeField] private GameObject _gridParent;
    [SerializeField] private GameObject _fieldOutline;
    [SerializeField] private GameObject _hexSectionParent;
    [SerializeField] private FollowMouseInRectBounds _advancedSelector;
    [SerializeField] private FollowMouseInRectBounds _basicSelector;

    private float _hue = 0.5f;
    private float _sat = 0.5f;
    private float _val = 0.5f;
    private float _currentHue { get { return _hue; } set { _hue = value; } }
    private float _currentSat { get { return _sat; } set { _sat = ClampToGrid(value); } }
    private float _currentVal { get { return _val; } set { _val = ClampToGrid(value); } }

    private bool _inputingHex;

    private Color _currentColor => Color.HSVToRGB(_currentHue, _currentSat, _currentVal);


    private Texture2D _hueTex;
    private Texture2D _satValTex;
    private bool _invoke;
    private bool _advanced;

    private void OnEnable()
    {
        _advancedSelector.FollowMouse = _advanced;
        _basicSelector.FollowMouse = !_advanced;
    }

    private void Start()
    {
        CreateTextures();
        _advancedSelector.enabled = false;
        _basicSelector.enabled = false;
        _invoke = false;
        UpdateHue();
        _invoke = true;
    }

    private void Update()
    {
        if (_advancedSelector.enabled || _basicSelector.enabled) {
            if (Input.GetMouseButtonUp(0)) StopSelecting();
            else UpdateCurrentColor();
        } 
    }

    private float ClampToGrid(float inputValue)
    {
        if (_advanced) return inputValue;
        float fraction = 1f / _gridResolution;
        var output = Mathf.Round(inputValue / fraction) * fraction;
        return output;
    }

    public void SetMode(bool advanced)
    {
        _advancedSelector.FollowMouse = advanced;
        _basicSelector.FollowMouse = !advanced;
        _hexSectionParent.SetActive(advanced);
        _fieldOutline.SetActive(advanced);
        _gridParent.SetActive(!advanced);
        _advanced = advanced;

        _advancedSelector.GetComponent<Image>().enabled = advanced;
        _basicSelector.GetComponent<Image>().enabled = !advanced;

        if (advanced) {
            _advancedSelector.transform.position = _basicSelector.transform.position;
            UpdateCurrentColor();
        }
        else {
            _basicSelector.transform.position = _advancedSelector.transform.position;
            UpdateCurrentColor();
        }
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
            var pos = _advancedSelector.GetNormalizedPositionFromCenter();
            if (_advanced) pos = _advancedSelector.GetNormalizedPositionFromCenter();
            

            if (!_advanced) {
                pos.x = ClampToGrid(pos.x); 
                pos.y = ClampToGrid(pos.y);
                //_basicSelector.SetPosition(pos);
                updateSatVal = SelectClosestGridSquare();
            }

            if (updateSatVal) {
                _currentSat = pos.x;
                _currentVal = pos.y;
            }

            _hexInput.UpdateText(_currentColor.ToHex());
        }
        _currentColorImg.color = _currentColor;
        if (_invoke) _onChangeColor.Invoke(_currentColor);
    }

    private bool SelectClosestGridSquare()
    {
        var selectorRTransform = _basicSelector.GetComponent<RectTransform>();

        var shortestDist = Mathf.Infinity;
        RectTransform bestSquare = null;
        foreach (RectTransform child in _squaresParent) {
            var dist = Vector2.Distance(child.anchoredPosition, selectorRTransform.anchoredPosition);
            if (dist < shortestDist) {
                bestSquare = child;
                shortestDist = dist;
            }
        }
        if (shortestDist > 500) return false;

        foreach (Transform child in _squaresParent) child.GetComponent<SelectableItem>().Deselect();
        bestSquare.GetComponent<SelectableItem>().Select();

        return true;
    }

    private Vector3 UIToWorldPos(RectTransform rTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        rTransform.GetWorldCorners(worldCorners);
        return (worldCorners[0] + worldCorners[2]) / 2f;
    }

    private void StopSelecting()
    {
        _advancedSelector.enabled = false;
        _basicSelector.enabled = false;
    }

    public void StartSelecting()
    {
        _inputingHex = false;
        if (_advanced) _advancedSelector.enabled = true;
        else _basicSelector.enabled = true;
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
            var col = Color.HSVToRGB(i / height, 1, _hueValue);
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

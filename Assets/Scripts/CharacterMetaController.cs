using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMetaController : MonoBehaviour
{
    [SerializeField] private SetMaterialColor _skin;
    [SerializeField] private Color _skinColor;
    [SerializeField] private FaceFeatureController _face;
    [SerializeField] private HairController _hair;
    [SerializeField] private EarController _ears;
    [SerializeField] private BodyCustomizer _bodyCustomizer;
    [SerializeField] private DataPanelController _dataPanel;
    
    [HideInInspector] public CharacterProfileData Data;

    public Color SkinColor => _skinColor;

    private void Start()
    {
        SetSkinColor(_skinColor);
    }

    public void LoadFromString(string input)
    {
        input = input.Replace("\n", "");
        var parts = input.Split('|');
        _face.LoadFromString(parts[0]);
        _hair.LoadFromString(parts[1]);
        _ears.LoadFromString(parts[2]);

        ColorUtility.TryParseHtmlString(parts[3], out _skinColor);
        SetSkinColor(_skinColor);

        _bodyCustomizer.LoadFromString(parts[4]);

        _dataPanel.LoadFromString(parts[5]);

    }

    public string GetSaveString()
    {
        var list = new List<string>
        {
            _face.GetSaveString(),
            _hair.GetSaveString(),
            _ears.GetSaveString(),
            _skinColor.ToHex(),
            _bodyCustomizer.GetSaveString(),
            Data.ToString()
        };

        return string.Join("|", list);
    }

    public void SetSkinColor(Color color)
    {
        _skinColor = color;
        _skin.SetColor(_skinColor);
        _ears.SetColor(_skinColor);
    }

}

using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class FacialFeature : FeatureObj
{
    [Header("Misc")]
    [SerializeField] private Material _refMaterial;
    [SerializeField] private Material _refMaterialDetail;

    private DecalProjector _projector;
    private FacialFeature _mirroredFeature;
    private FeatureSOData _replacementFeature;
    private Vector3 _expressionOffsets;
    private float _expressionRotOffset;

    [ReadOnly, HideInInspector] public FeatureCategory Category;

    private void OnValidate()
    {
        if (!Data) return;
        if (_projector == null) _projector = GetComponent<DecalProjector>();
        if (Data.Texture == null) {
            _projector.material = null;
            return;
        }
        if (_projector.material == null) MakeNewMaterial();
        
        _projector.material.SetTexture("Base_Map", Data.Texture);

        if (MirroredFeature && !MirroredFeature.gameObject.name.Contains("Mirror")) MirroredFeature.gameObject.name += " Mirror";
    }

    [ButtonMethod]
    public void SetOnTop()
    {
        _projector.material.SetInt("_DrawOrder", 1);
    }

    [ButtonMethod]
    public void SetInBack()
    {
        _projector.material.SetInt("_DrawOrder", -1);
    }

    public void SetScaleMode(bool inCharacterCreator)
    {
        if (inCharacterCreator) return;

        if (!_projector) _projector = GetComponent<DecalProjector>();

        _projector.scaleMode = DecalScaleMode.InheritFromHierarchy;
        if (_mirroredFeature != null) _mirroredFeature.SetScaleMode(inCharacterCreator);
    }

    public void SetExpression(ExpressionPieceData data)
    {
        _replacementFeature = data.Replacement;
        _expressionRotOffset = data.RotationOffset;
        _expressionOffsets = new Vector3(data.PositionXOffset, data.PositionYOffset, 0);
        if (IsMirroredVersion) _expressionOffsets.x *= -1;

        if (_mirroredFeature != null) _mirroredFeature.SetExpression(data);

        UpdateDisplay();
    }

    [ButtonMethod]
    private void MakeNewMaterial()
    {
        if (!_projector) _projector = GetComponent<DecalProjector>();
        var mat = new Material(_refMaterial);
        if (Tier == FeatureTier.DETAIL) mat = new Material(_refMaterialDetail);

        _projector.material = mat;
        if (Data.Texture == null) return;
        mat.name = Data.Texture.name + " (virtual)" + (IsMirroredVersion ? "(mirror)" : "");
        gameObject.name = Data.Texture.name;
        UpdateMaterial();
    }

    private void Start()
    {
        _projector = GetComponent<DecalProjector>();
    }

    public override void ConfigureFromString(string inputString)
    {
        base.ConfigureFromString(inputString);
        Category = (FeatureCategory) int.Parse(inputString.Substring(20, 1));

        if (Category == FeatureCategory.EXTRAS) {
            if (Tier == FeatureTier.BASE) SetInBack();
            else SetOnTop();
        }
    }

    public override string ToString()
    {
        var result = Data.Icon.name + "~";
        result += (Convert.ToInt32(Settings.MatchColor) * 3 + (int)Settings.Mirror);
        result += RoundToString(Settings.Hori) + RoundToString(Settings.Vert) + RoundToString(Settings.Size) + RoundToString(Settings.Angle) + Settings.Color.ToHex();
        result += (int)Tier;
        result += (int)Category;

        result = result.Replace("#", "");

        return result;
    }

    public void Set(FeatureSOData data, FeatureCategory category, FeatureTier priority)
    {
        Reset();
        Data = data;
        Category = category;
        Tier = priority;
        OnValidate();
        SetAll(data.DefaultSettings);
    }

    public void SetTier(FeatureTier tier)
    {
        if (MirroredFeature != null) MirroredFeature.As<FacialFeature>().SetTier(tier); 
        Tier = tier;
    }

    protected override void UpdateDisplay()
    {
        if (!this || !gameObject) return;

        if (_projector == null) {
            _projector = GetComponent<DecalProjector>();
            if (_projector == null) {
                Debug.LogError(gameObject.name + " can't find its decal projector");
            }

            _projector.material = null;
        }
        UpdatePos();
        UpdateAngle();

        if (Data.Texture == null) {
            return;
        }
        UpdateScale();
        UpdateMaterial();
        base.UpdateDisplay();

        var shouldShowMirrored = IsMirroredVersion && Settings.Mirror == MirrorType.RIGHT;
        var shouldShow = !IsMirroredVersion && Settings.Mirror == MirrorType.LEFT;

        if (shouldShow || shouldShowMirrored || Settings.Mirror == MirrorType.BOTH) {
            gameObject.SetActive(_replacementFeature == null || Tier == FeatureTier.BASE);
        }
    }


    private void UpdateAngle()
    {
        var angle = Mathf.Lerp(-180, 180, Settings.Angle);
        angle -= _expressionRotOffset;

        var euler = new Vector3(0, 0, IsMirroredVersion ? 1 - angle : angle);

        transform.localEulerAngles = euler;
    }

    private void UpdateScale()
    {
        var z = _projector.size.z;
        var newSize = Vector3.one * Mathf.Lerp(Data.SizeLimits.x, Data.SizeLimits.y, Settings.Size);
        newSize.z = z;
        _projector.size = newSize;
    }

    private void UpdatePos()
    {
        var pos = transform.localPosition;
        pos.x = Mathf.Lerp(Data.HoriLimits.x, Data.HoriLimits.y, IsMirroredVersion ? 1- Settings.Hori :  Settings.Hori);
        pos.y = Mathf.Lerp(Data.VertLimits.x, Data.VertLimits.y, Settings.Vert);

        pos += _expressionOffsets;

        transform.localPosition = pos;
    }

    private void UpdateMaterial()
    {
        var currentData = _replacementFeature ? _replacementFeature : Data;

        if (_projector.material == null || _projector.material.shader != _refMaterial.shader) MakeNewMaterial();
        _projector.material.SetTexture("_Base_Map", currentData.Texture);
        _projector.material.SetTexture("_colorMap", currentData.ColorMask);
        _projector.material.SetColor("_tint", Settings.Color);
        _projector.material.SetInt("_hasColor", currentData.ColorMask == null ? 0 : 1);
    }


    public override void SpawnMirror()
    {
        base.SpawnMirror();
        _mirroredFeature = MirroredFeature.As<FacialFeature>();
        _mirroredFeature.MakeNewMaterial();
        _mirroredFeature.SetScaleMode(_projector.scaleMode == DecalScaleMode.ScaleInvariant);
    }

    public override void MirroredSet(FeatureObjSettings settings)
    {
        if (_projector == null) _projector = GetComponent<DecalProjector>();

        _projector.uvScale = new Vector2(-1, 1);
        _projector.uvBias = new Vector2(1, 0);

        base.MirroredSet(settings);
    }

    [ButtonMethod]
    private void Reset()
    {
        OnValidate();
        Utils.SetDirty(this);
    }

    [ButtonMethod]
    private void SaveSettings()
    {
        var controller = GetComponentInParent<FaceFeatureController>();
        controller.SaveFeature(Data);
    }
}

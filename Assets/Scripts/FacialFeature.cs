using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class FacialFeature : FeatureObj
{

    [Header("Misc")]
    [SerializeField] private Material _refMaterial;

    private DecalProjector _projector;
    private FacialFeature _mirroredFeature;

    private void OnValidate()
    {
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
    private void MakeNewMaterial()
    {
        if (!_projector) _projector = GetComponent<DecalProjector>();
        var mat = new Material(_refMaterial);
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

    public void Set(FeatureData data)
    {
        Reset();
        Data = data;
        OnValidate();
        SetAll(data.DefaultSettings);
    }

    protected override void UpdateDisplay()
    {
        if ( _projector == null) _projector = GetComponent<DecalProjector>();
        UpdatePos();
        UpdateAngle();

        if (Data.Texture == null) return;
        UpdateScale();
        UpdateMaterial();
        base.UpdateDisplay();
    }


    private void UpdateAngle()
    {
        var angle = Mathf.Lerp(-180, 180, Settings.Angle);
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
        transform.localPosition = pos;
    }

    private void UpdateMaterial()
    {
        if (_projector.material == null) MakeNewMaterial();
        _projector.material.SetTexture("_Base_Map", Data.Texture);
        _projector.material.SetTexture("_colorMap", Data.ColorMask);
        _projector.material.SetColor("_tint", Settings.Color);
        _projector.material.SetInt("_hasColor", Data.ColorMask == null ? 0 : 1);
    }


    protected override void SpawnMirror()
    {
        base.SpawnMirror();
        _mirroredFeature = MirroredFeature.As<FacialFeature>();
        _mirroredFeature.MakeNewMaterial();
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
        Data = new FeatureData();
        OnValidate();
        Utils.SetDirty(this);
    }

    [ButtonMethod]
    private void SetBottomLeft()
    {
        Data.HoriLimits.x = transform.localPosition.x;
        Data.VertLimits.x = transform.localPosition.y;
        Utils.SetDirty(this);
    }

    [ButtonMethod]
    private void SetTopRight()
    {
        Data.HoriLimits.y = transform.localPosition.x;
        Data.VertLimits.y = transform.localPosition.y;
        Utils.SetDirty(this);
    }

    [ButtonMethod]
    private void SaveSettings()
    {
        var controller = GetComponentInParent<FaceFeatureController>();
        controller.SaveFeature(Data);
    }

}

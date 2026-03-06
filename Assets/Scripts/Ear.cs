using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ear : FeatureObj
{
    private CharacterMetaController _controller;
    private GameObject _model;

    private void Start()
    {
        foreach (Transform child in transform) if (child.gameObject != _model) Destroy(child.gameObject);
        _controller = GetComponentInParent<CharacterMetaController>();
        if (_model) UpdateDisplay();
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);   
    }

    public override void SetAsMirroredVersion()
    {
        base.SetAsMirroredVersion();
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        initilize();
    }

    public void initilize()
    {
        if (!Data) return;

        _model = Instantiate(Data.EarPrefab, transform);

        if (!IsMirroredVersion) UpdateMirror();

        SetAll(Data.DefaultSettings);
        UpdateDisplay();
    }

    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();

        if (!_model) return;
        if (_controller) _model.GetComponentInChildren<MeshRenderer>().materials[0].color = _controller.SkinColor;

        _model.transform.localScale = Vector3.one * Mathf.Lerp(Data.SizeLimits.x, Data.SizeLimits.y, Settings.Size);
        var euler = _model.transform.localEulerAngles;
        euler.x = Mathf.Lerp(Data.AngleLimits.x, Data.AngleLimits.y, Settings.Angle);
        _model.transform.localEulerAngles = euler;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var pos = _model.transform.localPosition;

        pos.x = Mathf.Lerp(Data.HoriLimits.x, Data.HoriLimits.y, Settings.Hori);
        pos.y = Mathf.Lerp(Data.VertLimits.x, Data.VertLimits.y, Settings.Vert);

        _model.transform.localPosition = pos;
    }

    public void SetData(FeatureSOData data)
    {
        Data = data;
        Destroy(_model);
        initilize();
        if (MirroredFeature != null) MirroredFeature.As<Ear>().SetData(data);
    }

    [ButtonMethod]
    private void Save()
    {
        GetComponentInParent<EarController>().Save(Data);
    }
}

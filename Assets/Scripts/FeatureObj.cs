using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeatureObj : MonoBehaviour
{
    [SerializeField] protected FeatureSOData Data;

    [SerializeField, ReadOnly] protected FeatureObjSettings Settings = new FeatureObjSettings();

    [HideInInspector] public bool IsMirroredVersion;
    protected FeatureObj MirroredFeature;

    public FeatureObjSettings GetSettings() => Settings;
    public FeatureSOData GetData() => Data;

    public void ConfigureFromString(string inputString)
    {
        var numString = inputString.Substring(1);

        int mirrorMatchNum = int.Parse(inputString[0].ToString());
        Settings.MatchColor = (mirrorMatchNum / 3) == 1;
        Settings.Mirror = (MirrorType)(mirrorMatchNum % 3);

        Settings.Hori = float.Parse(numString[..3]) / 1000;
        Settings.Vert = float.Parse(numString.Substring(3, 3)) / 1000;
        Settings.Size = float.Parse(numString.Substring(6, 3)) / 1000;
        Settings.Angle = float.Parse(numString.Substring(9, 3)) / 1000;

        ColorUtility.TryParseHtmlString("#" + numString.Substring(12, 6), out Settings.Color);
        UpdateDisplay();

        SetColor(Settings.Color);
    }

    public override string ToString()
    {
        var result = Data.Icon.name + "~";
        result += (Convert.ToInt32(Settings.MatchColor) * 3 + (int)Settings.Mirror);
        result += RoundToString(Settings.Hori) + RoundToString(Settings.Vert) + RoundToString(Settings.Size) + RoundToString(Settings.Angle) + Settings.Color.ToHex();

        result = result.Replace("#", "");

        return result;
    }

    private string RoundToString(float input )
    {
        input = Mathf.Clamp(input, 0.001f, 0.999f);
        var res = Mathf.FloorToInt(input * 1000).ToString();
        while (res.Length < 3) res = "0" + res;
        return res;
    }

    protected virtual void UpdateDisplay()
    {
        if (!IsMirroredVersion) UpdateMirror();
    }

    public virtual void SetColor(Color color)
    {
        color.a = 1;
        Settings.Color = color;
        UpdateDisplay();
        if (MirroredFeature != null) MirroredFeature.SetColor(color);
    }

    public virtual void MirroredSet(FeatureObjSettings settings)
    {
        Settings = new FeatureObjSettings(settings);

        UpdateDisplay();
    }

    protected void UpdateMirror()
    {
        if (!Application.isPlaying) return;

        if (MirroredFeature == null) SpawnMirror();

        gameObject.SetActive(Settings.Mirror == MirrorType.LEFT || Settings.Mirror == MirrorType.BOTH);
        MirroredFeature.gameObject.SetActive(Settings.Mirror == MirrorType.RIGHT || Settings.Mirror == MirrorType.BOTH);

        if (MirroredFeature) MirroredFeature.MirroredSet(Settings);
    }

    private void OnDestroy()
    {
        if (MirroredFeature) Destroy(MirroredFeature.gameObject);
    }

    protected virtual void SpawnMirror()
    {
        MirroredFeature = Instantiate(gameObject, transform.parent).GetComponent<FeatureObj>();
        MirroredFeature.SetAsMirroredVersion();
    }

    public virtual void CopyTo(FeatureObj feature)
    {
        feature.SetAll(Settings);
    }

    public virtual void SetAsMirroredVersion()
    {
        IsMirroredVersion = true;
    }

    public void SetMirrorTpe(MirrorType mirror)
    {
        if (Settings.Mirror == mirror) return;
        Settings.Mirror = mirror;
        UpdateDisplay();
    }

    public void SetAll(FeatureObjSettings settings)
    {
        Settings = new FeatureObjSettings(settings);
        UpdateDisplay();
    }

    public void SetHori(float hori)
    {
        Settings.Hori = hori;
        UpdateDisplay();
    }

    public void SetVert(float vert)
    {
        Settings.Vert = vert;
        UpdateDisplay();
    }

    public void SetSize(float size)
    {
        Settings.Size = size;
        UpdateDisplay();
    }

    public void SetAngle(float angle)
    {
        Settings.Angle = angle;
        UpdateDisplay();
    }
}

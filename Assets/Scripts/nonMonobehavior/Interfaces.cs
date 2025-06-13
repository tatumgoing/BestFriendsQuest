using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFeatureController
{
    public bool HasCurrent();
    public void CopySettingsToCurrent(FeatureObj original);
    public FeatureObj AddFeature(FeatureData data);
    public FeatureObj GetCurrent();
    public void Delete(FeatureObj feature);
    public List<FeatureObj> GetCurrentFeatures();
    public void SetCategory(FeatureCategory category);
    public void Select(FeatureObj feature);
    public List<FeatureData> GetAllOptions();
    public string GetSaveString();
    public void LoadFromString(string saveString);
}


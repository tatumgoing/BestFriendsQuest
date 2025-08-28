using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpressionPieceData
{
    public FeatureCategory Category;

    [Tooltip("This feature will replace all the 'base' features in this category")]
    public FeatureSOData Replacement;

    [Tooltip("All features in this category will by offset by this position amount, mirrored")]
    [Range(-0.5f, 0.5f)] public float PositionXOffset;

    [Tooltip("All features in this category will by offset by this position amount")]
    [Range(-0.5f, 0.5f)] public float PositionYOffset;

    [Tooltip("All features in this category will be rotated by this many degrees, mirrored")]
    [Range(-180, 180)] public float RotationOffset; 
}

[CreateAssetMenu(fileName = "Expression", menuName = "New Expression")]
public class ExpressionData : ScriptableObject
{
    public List<ExpressionPieceData> Data = new List<ExpressionPieceData>();
}

using UnityEngine;

[System.Serializable]
public class BoneSettingsData
{
    [SerializeField] private BoneName _bone;
    [SerializeField] private Vector3 _adjustDirection;
    [SerializeField] private Vector2 _scaleLimits = new Vector2(0.9f, 1.1f);
    [SerializeField] private Vector2 _distLimits = new Vector2(-0.05f, 0.05f);
    [SerializeField] private bool _symetricScale;
    [SerializeField] private bool _symetricDist;

    private Vector3 _targetPosition;
    private float _targetScale;

    public BoneName AffectedBone => _bone;
    public float ScaleMod => _targetScale;
    public Vector3 PositionOffset => _targetPosition;

    public void OnValidate()
    {
        if (_symetricScale) {
            var scale = _scaleLimits.y - 1;
            _scaleLimits.x = 1 - scale;
        }
        if (_symetricDist) {
            var dist = _distLimits.y;
            _distLimits.x = -dist;
        }
    }

    public void UpdateValue(float value)
    {
        //Debug.Log("update value called. dist limits: " + _distLimits + ", value: " + value + ". PositionOffset: " + PositionOffset);
        _targetScale = Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, value);
        Update(value);
    }

    public void Update(float percent)
    {
        ApplyScale(percent);
        ApplyDistance(percent);
    }

    public void ApplyScale(float scaleFactor)
    {
        _targetScale = Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, scaleFactor);
    }
    
    public void ApplyDistance(float distFactor)
    {
        var distance = Mathf.Lerp(_distLimits.x, _distLimits.y, distFactor) / 100;
        _targetPosition = _adjustDirection.normalized * distance;
        //Debug.Log("Applying distance: " + distFactor + ", distance: " + distance + ", _targetPosition: " + _targetPosition);
    }

    public void DrawGizmos(Vector3 drawPos)
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(drawPos, drawPos + _adjustDirection);

        Gizmos.DrawSphere(drawPos + _adjustDirection.normalized * _distLimits.x, 0.5f);
        Gizmos.DrawSphere(drawPos + _adjustDirection.normalized * _distLimits.y, 0.5f);
    }
}

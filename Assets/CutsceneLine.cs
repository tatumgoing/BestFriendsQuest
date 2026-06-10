using UnityEngine;

[System.Serializable]
public class CutsceneLine
{
    public CutsceneSpeaker Speaker;
    public string Line;

    public bool MetaLine;
    public bool HasExpression;
    public bool HasCamAngle;

    private Transform _lookTarget;
    private Vector3 _lookTargetPosition;

    public Vector3 LookPos => _lookTarget ? _lookTarget.position : _lookTargetPosition;
    public Expression Expression { get; private set; }

    public CutsceneLine(CutsceneSpeaker speaker, string line)
    {
        Speaker = speaker;
        Line = line;
    }

    public CutsceneLine(CutsceneSpeaker speaker)
    {
        MetaLine = true;
        Speaker = speaker;
        Line = "";
    }

    public void SetExpression(Expression expression)
    {
        Expression = expression;
        HasExpression = true;
    }

    public void SetCamAngle(Transform lookTarget)
    {
        HasCamAngle = true;
        _lookTarget = lookTarget;
    }
}

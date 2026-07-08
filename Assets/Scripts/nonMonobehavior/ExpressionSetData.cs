using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Expression { NEUTRAL, TALKING, BLINKING, HAPPY, SURPRISED, ANGRY, SAD}

[System.Serializable]
public class ExpressionSetItemData
{
    [HideInInspector] public string Name;
    [HideInInspector]public Expression ExpressionType;

    [DisplayInspector]public ExpressionData Expression;
}

[CreateAssetMenu(fileName = "New Expression Set", menuName = "Data/Expression Set")]
public class ExpressionSetData : ScriptableObject
{
    [SerializeField] private List<ExpressionSetItemData> _expressions = new List<ExpressionSetItemData>();

    [SerializeField] private Vector2 _blinkMinMax = new Vector2(3, 20);
    [SerializeField] private float _blinkDuration = 1;
    [SerializeField] private float _talkingSpeed;

    public float GetBlinkCooldown() => Random.Range(_blinkMinMax.x, _blinkMinMax.y);
    public float BlinkDuration => _blinkDuration;
    public float TalkingSpeed => _talkingSpeed;

    private void OnValidate()
    {
        var list = Utils.EnumToList<Expression>();

        for (int i = 0; i < list.Count; i++) {
            if (_expressions.Count <= i) _expressions.Add(new ExpressionSetItemData());

            _expressions[i].ExpressionType = list[i];
            _expressions[i].Name = list[i].ToString();
        }

        for (int i = _expressions.Count-1; i >= list.Count; i--) {
            _expressions.RemoveAt(i);
        }
    }

    public ExpressionData GetExpressionData(Expression type)
    {
        var selected = _expressions.Where(x => x.ExpressionType == type).ToList();
        if (selected.Count > 0) return selected.First().Expression;
        else return null;
    }
}

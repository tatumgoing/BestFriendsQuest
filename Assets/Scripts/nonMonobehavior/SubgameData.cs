using MyBox;
using UnityEngine;

public enum SubgameType { SITRRING, GRILLING, CHOPPING }

[System.Serializable]
public class SubgameData
{
    public float TimeLimit;
    public float TargetTime;
    public int Countdown = 3;

    public SubgameType Type;

    [Space()]
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public float MinStirSpeed;
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public float MaxStirSpeed;
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public Vector2 ChangeSpeedFrequency;
    
    //chopping
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopValue = 0.2f;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopPenalty = 0.1f;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopBarSpeed = 1;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopTargetPosition;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopTargetScale = 1.0f;
}

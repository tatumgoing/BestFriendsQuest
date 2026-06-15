using MyBox;
using UnityEngine;

public enum SubgameType { STIRRING, GRILLING, CHOPPING, BOILING, STEAMING, ROLLING }

[System.Serializable]
public class SubgameData
{
    public string IngredientName;
    public float TimeLimit;
    public float TargetTime;
    public int Countdown = 3;

    public SubgameType Type;

    [Space()]
    [ConditionalField(nameof(Type), false, false, SubgameType.STIRRING)] public float MinStirSpeed= 2.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.STIRRING)] public float MaxStirSpeed= 8.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.STIRRING)] public Vector2 ChangeSpeedFrequency = new Vector2 (2.0f,5.0f);
    
    //chopping
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopValue = 0.2f;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopPenalty = 0.1f;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopBarSpeed = 1;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopTargetPosition;
    [ConditionalField(nameof(Type), false, false, SubgameType.CHOPPING)] public float ChopTargetScale = 1.0f;

    //boiling
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilMinSpeed = -1.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilMaxSpeed = 2.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilAccSpeed = 2.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilDeccSpeed = 2.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilTargetPosition= 300.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.BOILING)] public float BoilTargetScale = 1.0f;

    //steaming
    [ConditionalField(nameof(Type), false, false, SubgameType.STEAMING)] public float SpawnRate = .5f;
    [ConditionalField(nameof(Type), false, false, SubgameType.STEAMING)] public float SteamValue = .2f;
    [ConditionalField(nameof(Type), false, false, SubgameType.STEAMING)] public float SteamPenalty = .1f;
    [ConditionalField(nameof(Type), false, false, SubgameType.STEAMING)] public float CloudLifetime = 2.5f;

    //rolling
    [ConditionalField(nameof(Type), false, false, SubgameType.ROLLING)] public float RollGreat = .2f;
    [ConditionalField(nameof(Type), false, false, SubgameType.ROLLING)] public float RollGood = .2f;
    [ConditionalField(nameof(Type), false, false, SubgameType.ROLLING)] public float RollPenalty = .1f;
    [ConditionalField(nameof(Type), false, false, SubgameType.ROLLING)] public float RollSpeed = 500.0f;
    [ConditionalField(nameof(Type), false, false, SubgameType.ROLLING)] public float SpeedUp = 100.0f;






}

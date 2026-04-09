using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameType { COOKING, GARDENING, FISHING}
public enum SubgameType { SITRRING, GRILLING}

[System.Serializable]
public class SubgameData
{
    public SubgameType Type;

    public float TimeLimit;
    public float TargetTime;
    public int countdown = 3;


    [Space()]
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public float MinStirSpeed;
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public float MaxStirSpeed;
    [ConditionalField(nameof(Type), false, false, SubgameType.SITRRING)] public Vector2 ChangeSpeedFrequency;
}


[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]

public class RecipeData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public float MaxScore;
    public int MoneyReward;

    public List<SubgameData> Subgames = new List<SubgameData>();
}

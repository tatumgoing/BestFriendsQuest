using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGroupData
{
    public List<ItemData> Items;
}

public enum ProblemType { SINGLE_ITEM, GENERAL_ITEM, MINIGAME}

[CreateAssetMenu(fileName = "Problem", menuName = "Problem", order = 2)]
public class ProblemData : ScriptableObject
{
    [TextArea(2, 10), SerializeField] private string _startingDialogue;
    [TextArea(2, 10), SerializeField] private string _completionDialogue;

    [Header("Details")]
    public ProblemType Type;
    [ConditionalField(nameof(Type), false, false, ProblemType.SINGLE_ITEM)] public ItemData SingleItem;
    [ConditionalField(nameof(Type), false, false, ProblemType.GENERAL_ITEM)] public ItemGroupData GeneralItems;
    [ConditionalField(nameof(Type), false, false, ProblemType.MINIGAME)] public MinigameType Minigame;

    [Header("Rewards")]
    public float RewardHappiness;
    public float RewardCurrency;

    [ReadOnly] public bool IsSolved;

    public string Dialogue()
    {
        if (IsSolved) return _completionDialogue;
        else return _startingDialogue;
    }
}

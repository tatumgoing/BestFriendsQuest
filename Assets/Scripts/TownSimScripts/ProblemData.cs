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
    [TextArea] public string Dialogue;

    [Header("Details")]
    public ProblemType Type;
    [ConditionalField(nameof(Type), false, false, ProblemType.SINGLE_ITEM)] public ItemData SingleItem;
    [ConditionalField(nameof(Type), false, false, ProblemType.GENERAL_ITEM)] public ItemGroupData GeneralItems;
    [ConditionalField(nameof(Type), false, false, ProblemType.MINIGAME)] public MinigameType Minigame;

    [Header("Rewards")]
    public float RewardHappiness;
    public float RewardCurrency;

    [HideInInspector] public bool IsSolved;

    private void OnValidate()
    {

    }
}

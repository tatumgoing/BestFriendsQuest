using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problem", order = 2)]
public class Problem : ScriptableObject
{
    public bool isSolved;

    public string problemDialogue;

    public Item desiredItem;

    public bool isMinigame;

    [Header("Rewards")]

    public float rewardHappiness;
    public float rewardCurrency;

}

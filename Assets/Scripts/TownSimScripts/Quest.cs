using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 3)]
public class Quest : ScriptableObject
{
    public Item unlockedItem;

    public float completionTime;

    public float relationshipRequirement;
}

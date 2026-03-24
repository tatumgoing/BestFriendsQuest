using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 3)]
public class Quest : ScriptableObject
{
    public bool completed= false;

    public ItemData unlockedItem;

    public float completionTime;

    public float relationshipRequirement;

    public float relationshipGain= .75f;

    public float relationshipLoss = -.25f;
}

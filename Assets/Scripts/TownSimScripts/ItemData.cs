using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Clothing, Food, Housing}

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemData : ScriptableObject
{
    public ID ID;

    public Sprite sprite;

    public string Name;

    public ItemType Type;

    [TextArea]
    public string Description;

    public float Cost;

    public bool unlocked;

    private void OnValidate()
    {
        if (Name.Length < 5) Name = name;
    }

    [ButtonMethod]
    public void RegenerateID()
    {
        ID.GenerateNew();
        Debug.Log("Generating new ID for " + name + ". All saved instances of this item have been erased.");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Clothing, Food, Housing}

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite sprite;

    public string Name;

    public ItemType Type;

    public string Description;

    public float Cost;

    public bool unlocked;

}

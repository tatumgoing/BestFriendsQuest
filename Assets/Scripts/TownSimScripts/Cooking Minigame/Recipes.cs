using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public Sprite icon;

    public string Name;

    public List<GameObject> minigames= new List<GameObject> ();

}

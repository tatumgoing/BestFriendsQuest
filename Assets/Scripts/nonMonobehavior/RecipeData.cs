using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class RecipeData : ScriptableObject
{
    public string Name;
    public Sprite Icon;

    [DisplayInspector] public List<GameObject> Minigames= new List<GameObject> ();
}

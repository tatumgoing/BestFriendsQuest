using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public Sprite Icon;

    public string Name;

    [DisplayInspector] public List<GameObject> Minigames= new List<GameObject> ();

}

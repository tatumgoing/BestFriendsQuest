using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Map Bundle", menuName ="Data/Map Bundle")]
public class MapData : ScriptableObject
{
    public string Name;
    public int NumIslands;
    public Difficulty Difficulty;
}

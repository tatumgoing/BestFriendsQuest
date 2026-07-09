using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Map Bundle", menuName ="Data/Map Bundle")]
public class MapData : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public Difficulty Difficulty;
    public int NumRequiredToUnlock;
    public List<Quest> Quests = new List<Quest>();

    public int NumIslands => Quests.Count;
}
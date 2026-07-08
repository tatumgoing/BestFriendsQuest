using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonalityType { CHILL, EAGER, CONFIDENT, BORED, ANGRY, SAD}

[CreateAssetMenu(fileName = "New Personality", menuName = "Data/Personality")]
public class PersonalityData : ScriptableObject
{
    public PersonalityType Type;

    [TextArea(3, 10)] public List<string> _defaultPhrases = new List<string>(); 

    public string GetRandomLine() => _defaultPhrases[Random.Range(0, _defaultPhrases.Count)];
}

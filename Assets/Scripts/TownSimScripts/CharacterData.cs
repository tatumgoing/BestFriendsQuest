using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CharacterData 
{
    TownGameManager gameManager = TownGameManager.i;

    [Header("Profile")]
    public string characterName;
    public int age;

    [Header("Happiness")]
    public float happiness;

    [Header("Relationships")]
    public Dictionary<CharacterData, float> relationships = new Dictionary<CharacterData, float>();

    [Header("House")]
    public GameObject house;

    [Header("Icon")]
    public Sprite characterIcon;

    public void UpdateIcon(Sprite icon)
    {
        characterIcon = icon;
    }

    public void UpdateHappiness(float newHappiness) {
        happiness= Mathf.Clamp(happiness + newHappiness, 0f, 100f);
    }
    public void UpdateRelationship(CharacterData reloCharacter, float newValue)
    {
        if (reloCharacter == this)
        {
            return;
        }
        else if (relationships.ContainsKey(reloCharacter))
        {
            relationships[reloCharacter] += newValue;
        }
        else
        {
            CreateRelationship(reloCharacter);
        }
    }

    public void CreateRelationship(CharacterData reloCharacter)
    {
        if (reloCharacter == this)
        {
            return;
        }
        else
        {
            relationships.Add(reloCharacter, 0);
        }
    }

}

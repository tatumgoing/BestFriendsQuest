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
    [Header("Profile")]
    public string characterName;
    public int age;

    [Header("Happiness")]
    public float happiness;

    [Header("Relationships")]
    public Dictionary<string, float> items = new Dictionary<string, float>();

    [Header("House")]
    public GameObject house;

    [Header("Icon")]
    public Sprite characterIcon;

    public void UpdateIcon(Sprite icon)
    {
        characterIcon = icon;
    }

}

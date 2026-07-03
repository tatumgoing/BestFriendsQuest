using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class MapOptionBFQuestData
{
    public string Name;
    public int NumIslands;
    public string Difficulty;
}

public class MapOptionBFQuest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _numIslandsText;
    [SerializeField] private TextMeshProUGUI _difficultyText;

    public void Initiailize(MapOptionBFQuestData data)
    {
        _nameText.text = data.Name;
        _numIslandsText.text = "Number of Islands: " + data.NumIslands;
        _difficultyText.text = data.Difficulty;
    }
}

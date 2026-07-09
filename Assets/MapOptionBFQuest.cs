using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MapOptionBFQuest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _numIslandsText;
    [SerializeField] private TextMeshProUGUI _difficultyText;

    public void Initiailize(MapData data)
    {
        _nameText.text = data.Name;
        _numIslandsText.text = "Number of Islands: " + data.NumIslands;
        _difficultyText.text = Utils.CapitalFirst(data.Difficulty.ToString());
        gameObject.SetActive(true);
    }
}

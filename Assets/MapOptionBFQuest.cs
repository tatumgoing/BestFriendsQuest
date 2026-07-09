using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapOptionBFQuest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _numIslandsText;
    [SerializeField] private TextMeshProUGUI _difficultyText;
    [SerializeField] private GameObject _lockedParent;
    [SerializeField] private TextMeshProUGUI _numRequiredText;

    private MapData _data;

    public MapData Data => _data;
    public bool Unlocked => !_lockedParent.activeInHierarchy;

    public void Initiailize(MapData data, bool unlocked)
    {
        _data = data;
        _nameText.text = data.Name;
        _numIslandsText.text = "Number of Islands: " + data.NumIslands;
        _difficultyText.text = Utils.CapitalFirst(data.Difficulty.ToString());
        _numRequiredText.text = "Complete " + data.NumRequiredToUnlock + " quests to unlock";
        _image.sprite = data.Image;

         _lockedParent.SetActive(!unlocked);


        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteamDemoUI : MonoBehaviour
{
    [SerializeField] private GameObject _areaUnlockParent;
    [SerializeField] private TextMeshProUGUI _areaUnlockText;
    [SerializeField] private TextMeshProUGUI _areaUnlockDescriptionText;
    [SerializeField] private GameObject _firstTimeController;

    private void Start()
    {
        _areaUnlockParent.SetActive(false);
        _firstTimeController.SetActive(CharacterManager.i.AllCharacters.Count == 0);
    }

    public void UnlockArea(string areaName, string description)
    {
        _areaUnlockText.text = areaName;
        _areaUnlockDescriptionText.text = description;
        _areaUnlockParent.SetActive(true);
    }
}

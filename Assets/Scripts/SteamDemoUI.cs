using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteamDemoUI : MonoBehaviour
{
    [SerializeField] private GameObject _areaUnlockParent;
    [SerializeField] private TextMeshProUGUI _areaUnlockText;
    [SerializeField] private GameObject _firstTimeController;

    private void Start()
    {
        _areaUnlockParent.SetActive(false);
        _firstTimeController.SetActive(CharacterManager.i.AllCharacters.Count == 0);
    }

    public void UnlockArea(string areaName)
    {
        _areaUnlockText.text = areaName;
        _areaUnlockParent.SetActive(true);
    }
}

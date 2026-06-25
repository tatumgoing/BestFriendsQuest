using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteamDemoUI : MonoBehaviour
{
    [SerializeField] private GameObject _areaUnlockParent;
    [SerializeField] private TextMeshProUGUI _areaUnlockText;

    private void Start()
    {
        _areaUnlockParent.SetActive(false);
    }

    public void UnlockArea(string areaName)
    {
        _areaUnlockText.text = areaName;
        _areaUnlockParent.SetActive(true);
    }
}

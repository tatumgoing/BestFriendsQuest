using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputField;
    [SerializeField] private SelectableItem _confirmButton;

    private void OnEnable()
    {
        _confirmButton.SetDisabled(true);
    }

    private void Update()
    {
        _confirmButton.SetDisabled(_inputField.text.Length <= 1);
    }

    public void Confirm()
    {
        SaveSystem.SaveToDialogueDict("playerName", _inputField.text.Trim());
        gameObject.SetActive(false);
    }
}

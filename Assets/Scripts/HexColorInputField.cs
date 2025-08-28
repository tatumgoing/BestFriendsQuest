using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HexColorInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Sound _errorSound;
    private ColorMenuController _controller;
    private string _lastValid;

    private void Start()
    {
        _errorSound = Instantiate(_errorSound);
        _controller = GetComponentInParent<ColorMenuController>();
    }

    public void UpdateText(string current)
    {
        //print("updating hex text: " + current);
        current = current.Replace("#", "").ToLower();
        _lastValid = current;
        _inputField.text = current.ToUpper();
    }

    public void CheckText()
    {
        string current = _inputField.text.ToLower();
        if (current == _lastValid) return;

        if (current.Length > 6) {
            ResetToLastValid();
            return;
        }

        if (current.Length == 0) return;

        int newLetter = current[^1];
        if (newLetter < 48) ResetToLastValid();
        else if (newLetter > 57 && newLetter < 97) ResetToLastValid();
        else if (newLetter > 102) ResetToLastValid();
        else {
            UpdateText(current);
            string old = current;
            while (current.Length < 6) current = "0" + current;
            current = "#" + current;
            _controller.SetFromHexCode(current);
        }
    }

    private void ResetToLastValid()
    {
        _inputField.text = _lastValid.ToUpper();
        _errorSound.Play();
    }
}

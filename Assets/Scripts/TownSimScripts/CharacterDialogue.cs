using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    [SerializeField] private List<string> _randomLines = new List<string>();
    [SerializeField] private TextMeshProUGUI _textBox;

    public void ShowRandomText() => ShowText(_randomLines[Random.Range(0, _randomLines.Count)]);

    public void ShowText(string text)
    {
        _textBox.text = text;
        gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        gameObject.SetActive(false);
    }
}

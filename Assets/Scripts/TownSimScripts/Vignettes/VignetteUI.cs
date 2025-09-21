using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VignetteUI : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    public TMP_Text dialogueText;

    void Start()
    {
        HideTextBox();
    }
    public void ChangeText(string newText)
    {
        dialogueText.text = newText;
    }

    public void ClearText()
    {
        dialogueText.text = " ";
    }

    public void HideTextBox()
    {
        textBox.SetActive(false);
    }

    public void ShowTextBox()
    {
        textBox.SetActive(true);
    }
}

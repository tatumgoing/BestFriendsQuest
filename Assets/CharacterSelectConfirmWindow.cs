using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectConfirmWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField, Tooltip("'NAME' will get replaced with the character name")] private string _promptString;
    [SerializeField] private Image _portraitImg;

    private MinigameController _controller;
    private ID _id;

    public void Display(ID id, MinigameController controller)
    {
        var prompt = _promptString.Replace("NAME", CharacterManager.i.GetNameFormatted(id));
        _promptText.text = prompt;
        _portraitImg.sprite = CharacterManager.i.GetPortrait(id);

        _controller = controller;
        _id = id;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Called from the 'confirm' button on the confirm window, starts the minigame with the selected character and then hides the confirm window
    /// </summary>
    public void Confirm()
    {
        _controller.SelectCharacter(_id);
        Hide();
    }

    /// <summary>
    /// Called when the 'back' button is pressed on the confirm window or when the confirm window is closed after confirming a character selection
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class MinigameButtonNames
{
    [HideInInspector] public string DisplayName;
    [HideInInspector] public MinigameType Type;
    public string ButtonText = "Start Cooking";
}

public class CharacterDialogue : MonoBehaviour
{
    [SerializeField] private List<string> _randomLines = new List<string>();
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private GameObject _minigameButtons;
    [SerializeField] private TextMeshProUGUI _minigameConfirmButtonText;
    [SerializeField] private List<MinigameButtonNames> _buttonStrings = new List<MinigameButtonNames>();

    private ID _id;

    public void StartMinigame() => TownGameManager.i.QuickStartMinigame(_id);
    private void ShowRandomText() => ShowText(_randomLines[Random.Range(0, _randomLines.Count)]);

    private void OnValidate()
    {
        var options = Utils.EnumToList<MinigameType>();

        while (_buttonStrings.Count < options.Count) _buttonStrings.Add(new MinigameButtonNames());
        for (int i = _buttonStrings.Count-1; i >= 0; i--) {
            if (i > options.Count) {
                _buttonStrings.RemoveAt(i);
                continue;
            }
            _buttonStrings[i].Type = options[i];
            _buttonStrings[i].DisplayName = options[i] + ": " + _buttonStrings[i].ButtonText;
        }
    }

    public void Talk(ID id)
    {
        _id = id;
        var characterDialogue = CharacterManager.i.GetDialogue(id);

        if (characterDialogue == "") ShowRandomText();
        else ShowText(characterDialogue);

        var currentProblem = CharacterManager.i.GetProblem(id);
        var isProblemMinigame = currentProblem && currentProblem.Type == ProblemType.MINIGAME;
        if (isProblemMinigame) {
            _minigameConfirmButtonText.text = _buttonStrings.Where(x => x.Type == currentProblem.Minigame).First().ButtonText;
        }
        
        _minigameButtons.SetActive(isProblemMinigame);
        _closeButton.SetActive(!isProblemMinigame);
    }

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

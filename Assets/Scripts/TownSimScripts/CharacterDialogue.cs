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
    [SerializeField] private Sound _beepSound;
    [SerializeField] private float _letterDelay = 0.02f;


    [SerializeField] private List<string> _randomLines = new List<string>();
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private GameObject _minigameButtons;
    [SerializeField] private TextMeshProUGUI _minigameConfirmButtonText;
    [SerializeField] private List<MinigameButtonNames> _buttonStrings = new List<MinigameButtonNames>();

    private ID _id;
    private float _letterCountdown;
    private string _targetText;

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

    private void Start()
    {
        _beepSound = Instantiate(_beepSound);
    }

    private void Update()
    {
        if (_targetText.Length == 0) return;

        _letterCountdown -= Time.deltaTime;
        if (_letterCountdown <= 0) {
            _letterCountdown = _letterDelay;
            var nextLetter = _targetText[0];
            _textBox.text += nextLetter;
            _targetText = _targetText.Substring(1);
            if (nextLetter != ' ') _beepSound.Play(restart:false);
        }
    }

    public void Talk(ID id)
    {
        _id = id;
        var characterDialogue = CharacterManager.i.GetDialogue(id);

        if (characterDialogue == "") ShowRandomText();
        else ShowText(characterDialogue);

        var currentProblem = CharacterManager.i.GetProblem(id);
        if (currentProblem && currentProblem.IsSolved) {
            CharacterManager.i.GiveProblemRewards(id);
        }
        
        bool isProblemMinigame = currentProblem && currentProblem.Type == ProblemType.MINIGAME;
        if (isProblemMinigame && !currentProblem.IsSolved) {
            _minigameConfirmButtonText.text = _buttonStrings.Where(x => x.Type == currentProblem.Minigame).First().ButtonText;
        }
        
        _minigameButtons.SetActive(isProblemMinigame && !currentProblem.IsSolved);
        _closeButton.SetActive(!isProblemMinigame || (currentProblem && currentProblem.IsSolved));
    }

    public void ShowText(string text)
    {
        _letterCountdown = 0;
        _textBox.text = "";
        _targetText = text;
        gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        GetComponentInParent<RoomUIController>().Show(_id);
        gameObject.SetActive(false);
    }
}

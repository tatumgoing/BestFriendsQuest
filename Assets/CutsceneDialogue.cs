using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public enum CutsceneSpeaker { SPEAKER_1, SPEAKER_2};

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private float _letterDelayTime;
    [SerializeField] private GameObject _c1Parent;
    [SerializeField] private GameObject _c2Parent;
    [SerializeField] private TextMeshProUGUI _c1Name;
    [SerializeField] private TextMeshProUGUI _c2Name;

    private int _currentLineIndex;
    private string _currentLineLeft = "";
    private float _letterCountdown;
    private List<CutsceneLine> _dialogue = new List<CutsceneLine>();
    private CutsceneLine _currentLine => _dialogue[_currentLineIndex];

    private void Update()
    {
        if (_currentLineLeft.Length == 0) return;

        _letterCountdown -= Time.deltaTime;
        if (_letterCountdown <= 0) {
            _letterCountdown = _letterDelayTime;

            _textBox.text += _currentLineLeft[0];
            if (_currentLineLeft.Length > 0) _currentLineLeft = _currentLineLeft.Substring(1);
        }
    }

    public async void StartDialogue(List<string> lines, List<string> names)
    {
        gameObject.SetActive(true);

        if (names.Count > 0) _c1Name.text = names[0];
        if (names.Count > 1) _c2Name.text = names[1];

        _textBox.text = "";
        _currentLineIndex = -1;
        _dialogue.Clear();
        ParseDialogue(lines);

        await Task.Delay(1000);

        Next();
    }

    private void ParseDialogue(List<string> rawLines)
    {
        foreach (var line in rawLines) {
            var parts = line.Split(":");
            var lineData = parts[0].ToUpper().Trim();

            var newLine = new CutsceneLine(CutsceneSpeaker.SPEAKER_1, parts[1]);
            if (lineData.Contains("C2")) newLine.Speaker = CutsceneSpeaker.SPEAKER_2;

            _dialogue.Add(newLine);
        }
    }

    public void Next()
    {
        if (_currentLineLeft.Length > 0) {
            _textBox.text += _currentLineLeft;
            _currentLineLeft = "";
            return;
        }

        _currentLineIndex += 1;
        _textBox.text = "";
        if (_currentLineIndex >= _dialogue.Count) {
            EndConversation();
            return;
        }
        
        _currentLineLeft = _currentLine.Line;
        _c1Parent.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_1);
        _c2Parent.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_2);
    }

    private void EndConversation()
    {
        CutsceneManager.i.EndCutscene();
        gameObject.SetActive(false);
    }
}

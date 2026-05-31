using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField, TextArea(2, 10)] private List<string> _dialogue = new List<string>();
    [SerializeField] private float _letterDelayTime;

    private int _currentLineIndex;
    private string _currentLineLeft = "";
    private float _letterCountdown;

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

    public async void StartDialogue()
    {
        gameObject.SetActive(true);

        _textBox.text = "";
        _currentLineIndex = -1;
        await Task.Delay(1000);

        Next();
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
        if (_currentLineIndex >= _dialogue.Count) EndConversation();
        else _currentLineLeft = _dialogue[_currentLineIndex];
    }

    private void EndConversation()
    {
        gameObject.SetActive(false);
    }
}

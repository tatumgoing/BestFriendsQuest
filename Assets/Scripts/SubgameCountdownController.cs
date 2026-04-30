using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubgameCountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private List<string> _countdownStrings = new List<string>();
    [SerializeField] private SubgameController _controller;
    [SerializeField] private TextMeshProUGUI _countdownHeader;

    private float _timeLeft;
    private int _lastSecond;
    private float _maxSeconds;

    public void StartCountdown(SubgameType type, int seconds = 3)
    {
        _timeLeft = seconds;
        _lastSecond = seconds;
        _maxSeconds = seconds;
        UpdateText();
        gameObject.SetActive(true);

        var countdownString = "Time to Cook!";
        var verb = "";
        if (type == SubgameType.CHOPPING) verb = "chop";
        if (type == SubgameType.SITRRING) verb = "stir";
        if (type == SubgameType.BOILING) verb = "boil";
        if (type == SubgameType.GRILLING) verb = "grilling";
        if (verb != "") countdownString = countdownString.Replace("Cook", verb);
        _countdownHeader.text = countdownString;
    }

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0) {
            gameObject.SetActive(false);
            _controller.CompleteCountdown();
            return;
        }

        var newSeconds = Mathf.CeilToInt(_timeLeft);
        if (newSeconds != _lastSecond) {
            _lastSecond = newSeconds;
            UpdateText();
        }
    }

    private void UpdateText() 
    {
        _countdownText.text = _countdownStrings[_countdownStrings.Count - _lastSecond];
    }
}

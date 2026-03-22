using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubgameCountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private List<string> _countdownStrings = new List<string>();
    [SerializeField] private SubgameController _controller;

    private float _timeLeft;
    private int _lastSecond;
    private float _maxSeconds;

    public void StartCountdown(int seconds = 3)
    {
        _timeLeft = seconds;
        _lastSecond = seconds;
        _maxSeconds = seconds;
        UpdateText();
        gameObject.SetActive(true);
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

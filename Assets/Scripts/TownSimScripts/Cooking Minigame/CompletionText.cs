using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompletionText : MonoBehaviour
{
    [SerializeField] private SubgameController _controller;
    [SerializeField] private Sound _completeSound;
    [SerializeField] private TextMeshProUGUI _completionText;
    [SerializeField] private List<string> _completionStrings;

    public void Show(float scorePercent)
    {
        if (!_completeSound.Instantialized) _completeSound = Instantiate(_completeSound);
        _completeSound.Play();

        var stringIndex = Mathf.FloorToInt(scorePercent * _completionStrings.Count);
        if (scorePercent > 0.99f) _completionText.text = "Perfect!";
        else _completionText.text = _completionStrings[stringIndex];

        gameObject.SetActive(true);
    }

    public void FinishAnimation()
    {
        _controller.StartCurrentSubgame();
        gameObject.SetActive(false);
    }
}

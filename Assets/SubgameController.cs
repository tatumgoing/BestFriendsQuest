using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] 
public class SubGameListData
{
    public SubgameType Type;
    public Subgame Subgame;
}

public class SubgameController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<SubGameListData> _options = new List<SubGameListData>();

    [Header("SubMenus")]
    [SerializeField] private SubgameCountdownController _countdownTimer;
    [SerializeField] private CompletionText _completionText;

    [Header("Sounds")]
    [SerializeField] private Sound _tickSound;

    [Header("Misc References")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private Slider _slider;


    private float _totalTime;
    private float _timeLeft;
    private float _targetSliderPos;
    private int _currentSeconds;
    private bool _initialized;
    private int _subgameIndex;
    private float _totalScore;
    private RecipeData _currentRecipe;
    private Subgame _currentSubgame;

    private SubgameData _currentSubgameData => _currentRecipe.Subgames[_subgameIndex];

    public float TimeLeftPercent => _timeLeft/_totalTime;
    public void UpdateSlider(float targetPercent) => _targetSliderPos = targetPercent;  
    
    private void Update()
    {
        _slider.value = Mathf.Lerp(_slider.value, _targetSliderPos, 7.5f * Time.deltaTime);

        if (_currentSubgame == null || !_currentSubgame.gameObject.activeInHierarchy) return;

        _timeLeft -= Time.deltaTime;
        _timerFillImage.fillAmount = _timeLeft / _totalTime;

        var newSeconds = Mathf.CeilToInt(_timeLeft);
        if (newSeconds != _currentSeconds) {
            _tickSound.Play();
            _currentSeconds = newSeconds;
            _timerText.text = _currentSeconds.ToString();   
        }

        if (_timeLeft <=0) {
            CompleteSubgame();
        }
    }

    public void CompleteSubgame()
    {
        _currentSubgame.gameObject.SetActive(false);
        _currentSubgame = null;

        var score = _targetSliderPos;
        _totalScore += score;

        _subgameIndex++;
        _completionText.Show(score);
    }

    private void FinishMinigame()
    {
        print("FINISHING MINIGAME!");
        _currentSubgame = null;
        gameObject.SetActive(false);
    }

    public void StartSubgame(RecipeData recipe)
    {
        if (!_initialized) Initialize();

        _currentRecipe = recipe;
        _subgameIndex = 0;

        _currentSubgame = null;
        gameObject.SetActive(true);

        StartCurrentSubgame();
    }

    /// <summary>
    /// Called from completionText so that the next one starts once the completion text is finished animating
    /// </summary>
    public void StartCurrentSubgame()
    {
        if (_subgameIndex >= _currentRecipe.Subgames.Count) {
            ShowResults();
            return;
        }

        _totalTime = _currentSubgameData.TimeLimit;
        _timeLeft = _currentSubgameData.TimeLimit;

        _totalScore = 0;
        _slider.value = 0;
        _targetSliderPos = 0;

        foreach (var o in _options) {
            if (o.Type == _currentSubgameData.Type) {
                _currentSubgame = o.Subgame;
                _countdownTimer.StartCountdown(_currentSubgameData.countdown);
            }
            else {
                o.Subgame.gameObject.SetActive(false);
            }
        }
    }

    private void ShowResults()
    {

    }

    public void CompleteCountdown()
    {
        _currentSubgame.StartSubgame(_currentSubgameData);
    }

    private void Initialize()
    {
        _initialized = true;
        _tickSound = Instantiate(_tickSound); 
    }
    
}

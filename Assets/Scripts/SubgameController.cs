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
    [SerializeField] private MinigameResultsScreen _results;
    [SerializeField] private SubgameInstructions _instructions;

    [Header("Sounds")]
    [SerializeField] private Sound _tickSound;

    [Header("Misc References")]
    [SerializeField] private GameObject _timerParent;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _timerFillImage;
    [SerializeField] private Slider _slider;
    [SerializeField] private RestrauntController _areaController;


    private float _totalTime;
    private float _timeLeft;
    private float _targetSliderPos;
    private int _currentSeconds;
    private bool _initialized;
    private int _subgameIndex;
    private float _totalScore;
    private RecipeData _currentRecipe;
    private Subgame _currentSubgame;
    private ID _character;
    private ID _recipient;
    private bool _isProblem;

    public RestrauntController AreaController => _areaController;
    private SubgameData _currentSubgameData => _currentRecipe.Subgames[_subgameIndex];
    public float TimeLeftPercent => _timeLeft/_totalTime;
    public void UpdateSlider(float targetPercent)
    {
        //print("Updating Slider: " + targetPercent + ", current: " + _targetSliderPos);
        _targetSliderPos = targetPercent;
    }
    
    private void Update()
    {
        if (_targetSliderPos == 0) _slider.value = 0;
        else _slider.value = Mathf.Lerp(_slider.value, _targetSliderPos, 7.5f * Time.deltaTime);

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
        _totalScore += score/_currentRecipe.Subgames.Count;
        //print("Completed subgame. score: " +  score + ", totalScore: " + _totalScore);

        _subgameIndex++;
        _completionText.Show(score);

    }

    private void FinishMinigame()
    {
        gameObject.SetActive(false);
    }

    public void StartMinigame(RecipeData recipe, ID character, ID recipient, bool isProblem)
    {
        if (!_initialized) Initialize();

        _isProblem = isProblem;
        _character = character;
        _recipient = recipient;
        _timerParent.SetActive(true);
        _results.gameObject.SetActive(false);
        _currentRecipe = recipe;

        _subgameIndex = 0;
        _totalScore = 0;

        _currentSubgame = null;
        gameObject.SetActive(true);

        StartCurrentSubgame();
    }

    /// <summary>
    /// Called from completionText so that the next one starts once the completion text is finished animating
    /// </summary>
    public void StartCurrentSubgame()
    {
        //print("trying to start next selected subgame");
        if (_subgameIndex >= _currentRecipe.Subgames.Count) {
            _currentSubgame = null;
            ShowResults();
            return;
        }

        _totalTime = _currentSubgameData.TimeLimit;
        _timeLeft = _currentSubgameData.TimeLimit;

        _slider.value = 0;
        _targetSliderPos = 0;

        foreach (var o in _options) {
            if (o.Type == _currentSubgameData.Type) {
                _currentSubgame = o.Subgame;
                _instructions.Show(_currentSubgameData.Type);
            }
            else {
                o.Subgame.gameObject.SetActive(false);
            }
        }
    }

    public void StartCountdown()
    {
        _countdownTimer.StartCountdown(_currentSubgameData.Type, _currentSubgameData.Countdown);
    }

    private async void ShowResults()
    {
        //highscores
        Dictionary<string, float> tempDict = SaveSystem.LoadHighscoreDictionary("Cooking");

        if (!tempDict.ContainsKey(_currentRecipe.name)) tempDict.Add(_currentRecipe.name, _totalScore);
        else if (tempDict[_currentRecipe.Name] <= _totalScore)
        {
            tempDict[_currentRecipe.Name] = _totalScore;
            SaveSystem.SaveHighscoreDictionary("Cooking", tempDict);

            //add a new highscore banner later
        }

        CharacterManager.i.IncreaseHappiness(_recipient, _totalScore * _currentRecipe.HappinessReward);
        if (_character != _recipient) {
            CharacterManager.i.IncreaseRelationship(_character, _recipient, _currentRecipe.RelationshipReward * _totalScore);
        }

        _timerParent.SetActive(false);
        await _results.ShowScore(_totalScore, _currentRecipe, _character, _recipient, _isProblem);
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

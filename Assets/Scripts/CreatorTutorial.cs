using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum CCTutorialTriggerType { NEXT, PRESS_BUTTON, NUMBER_FEATURES, GAMEOBJECT}

[System.Serializable] 
public class TutorialData
{
    [HideInInspector] public string DisplayName;
    [TextArea(3, 10)] public string Text;

    [Header("ContinueTrigger")]
    public CCTutorialTriggerType TriggerType;
    [ConditionalField(nameof(TriggerType), true, false, CCTutorialTriggerType.NEXT)] public bool Auto;
    [ConditionalField(nameof(TriggerType), false, false, CCTutorialTriggerType.PRESS_BUTTON)] public SelectableItem Button;
    [ConditionalField(nameof(TriggerType), false, false, CCTutorialTriggerType.NUMBER_FEATURES)] public int MinNumber;
    [ConditionalField(nameof(TriggerType), false, false, CCTutorialTriggerType.GAMEOBJECT)] public GameObject GameObject;

    [Space()]
    public GameObject HighlightParent;
}

public class CreatorTutorial : MonoBehaviour
{
    [SerializeField] private List<TutorialData> _steps;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private CharacterMetaController _character; 

    [Header("Typewriter effect")]
    [SerializeField] private float _letterDelayTime = 0.04f;

    [Header("Animation")]
    [SerializeField] private Animator _hideButtonAnimator;
    [SerializeField] private Animator _showButtonAnimator;
    [SerializeField] private Animator _journalAnimator;
    [SerializeField] private GameObject _nextButton;

    [Header("Sounds")]
    [SerializeField] private Sound _letterSound;
    [SerializeField] private Sound _completeStepSound;

    private float _letterCooldown;

    private TutorialData _current => _steps[0];

    private void OnValidate()
    {
        for (int i = 0; i < _steps.Count; i++) {
            _steps[i].DisplayName = (i + 1) + ": " + _steps[i].Text;
            if (_steps[i].TriggerType == CCTutorialTriggerType.NEXT) {
                _steps[i].Auto = false;
            }
        }
    }

    private void OnEnable()
    {
        _showButtonAnimator.SetTrigger("Hide");

        foreach (var s in _steps) {
            if (s.HighlightParent) s.HighlightParent.SetActive(false);
        }

        StartCurrent();
    }

    private void Start()
    {
        _letterSound = Instantiate(_letterSound);
        _completeStepSound = Instantiate(_completeStepSound);
    }

    private void Update()
    {
        if (_steps.Count == 0) return;

        if (_current.TriggerType == CCTutorialTriggerType.NUMBER_FEATURES && _character.NumFeatures >= _current.MinNumber) {
            CompleteStep();
        }

        _letterCooldown -= Time.deltaTime;
        var stillAnimating = _current.Text.Length > _textBox.maxVisibleCharacters;
        if (_letterCooldown <= 0 && stillAnimating) {
            _textBox.maxVisibleCharacters += 1;
            _letterCooldown = _letterDelayTime;
            _letterSound.Play(restart: false);
        }

        _nextButton.SetActive(_current.TriggerType == CCTutorialTriggerType.NEXT && !stillAnimating);
    }

    public void SkipAnimation()
    {
        _textBox.maxVisibleCharacters = _textBox.text.Length;
        _letterSound.Play(restart: false);
    }

    public void CompleteStep()
    {
        _journalAnimator.SetTrigger("Throb");
        if (_current.TriggerType == CCTutorialTriggerType.PRESS_BUTTON) {
            _current.Button.OnSelect.RemoveListener(CompleteStep);
        }
        if (_current.HighlightParent) _current.HighlightParent.SetActive(false);


        _completeStepSound.Play();
        _steps.RemoveAt(0);
        if (_steps.Count > 0) {
            StartCurrent();
            Show();
        }
    }

    private void StartCurrent()
    {
        _textBox.text = _current.Text;
        _textBox.maxVisibleCharacters = 0;

        if (_steps.Count == 0) {
            Hide();
            _showButtonAnimator.SetTrigger("Hide");
        }

        if (_current.HighlightParent) _current.HighlightParent.SetActive(true);
        if (_current.TriggerType == CCTutorialTriggerType.PRESS_BUTTON) {
            _current.Button.OnSelect.AddListener(CompleteStep);
        }
    }

    public void Show()
    {
        _hideButtonAnimator.SetTrigger("Show");
        _showButtonAnimator.SetTrigger("Hide");
        _journalAnimator.SetBool("Hidden", false);
    }

    public void Hide()
    {
        _hideButtonAnimator.SetTrigger("Hide");
        _showButtonAnimator.SetTrigger("Show");
        _journalAnimator.SetBool("Hidden", true);
    }


}

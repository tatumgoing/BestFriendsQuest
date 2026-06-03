using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameResultsListEntry : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _portrait2;
    [SerializeField] private Slider _slider;
    [SerializeField] private AnimationCurve _sliderCurve;
    [SerializeField] private Transform _starParent;
    [SerializeField] private Color _inactiveStarColor;
    [SerializeField] private Color _activeStarColor;
    [SerializeField] private GameObject _coinParent;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _sliderLabel;

    private float _sliderStartValue;
    private float _sliderTarget;
    private float _totalAnimationTime;
    private float _animationCountdown;

    private void Update()
    {
        if (_animationCountdown <= 0) return;

        _animationCountdown -= Time.deltaTime;
        var progress = _animationCountdown / _totalAnimationTime;
        progress = _sliderCurve.Evaluate(1 - progress);
        _slider.value = Mathf.Lerp(_sliderStartValue, _sliderTarget, progress);

        if (_animationCountdown <= 0 && !_portrait2.gameObject.activeInHierarchy) {
            var text = "+";
            if (_sliderTarget - _sliderStartValue > 0.25f) text = "++";
            if (_sliderTarget - _sliderStartValue < 0f) text = "-";
            if (_sliderTarget - _sliderStartValue < -0.25f) text = "--";
            _sliderLabel.text = text;
        }
    }

    public void Initialize(ID chef, ID recipient, float relationshipStart, float relationshipDelta, float animationTime = 0.5f)
    {
        HideAll();

        _slider.gameObject.SetActive(true);
        _portrait.gameObject.SetActive(true);
        _portrait2.gameObject.SetActive(true);

        _portrait.sprite = CharacterManager.i.GetPortrait(chef);
        _portrait2.sprite = CharacterManager.i.GetPortrait(recipient);

        _slider.value = relationshipStart - Mathf.Floor(relationshipStart);
        _sliderStartValue = _slider.value;
        _sliderTarget = relationshipDelta + _slider.value;
        _animationCountdown = _totalAnimationTime = animationTime;
    }

    /// <summary>
    /// Shows the money earned
    /// </summary>
    public void Initialize(int money)
    {
        HideAll();
        _coinParent.SetActive(true);
        _label.gameObject.SetActive(true);

        _label.text = "Reward";
        _coinText.text = money.ToString();
        
    }

    /// <summary>
    /// Shows the star rating (0-3 stars)
    /// </summary>
    public void Initialize(float score)
    {
        HideAll();
        _starParent.gameObject.SetActive(true);
        _label.gameObject.SetActive(true);

        _label.text = "Score";

        for (int i = 0; i < _starParent.childCount; i++) {
            var star = _starParent.GetChild(i).GetComponent<Image>();
            var active = (score >= 1 / _starParent.childCount);
            star.color = active ? _activeStarColor : _inactiveStarColor;
        }
    }

    /// <summary>
    /// Shows the happiness effect on a character, with their portrait and a slider indicating the amount of happiness change
    /// </summary>
    public void Initialize(ID character, float happinessIncrease, float animationTime = 0.5f)
    {
        HideAll();
        _slider.gameObject.SetActive(true);

        _portrait.gameObject.SetActive(true);
        _portrait.sprite = CharacterManager.i.GetPortrait(character);
        _slider.value = (CharacterManager.i.GetHappiness(character) / 100f) - (happinessIncrease/100f);

        _sliderStartValue = _slider.value;
        _sliderTarget = CharacterManager.i.GetHappiness(character) / 100f;
        _animationCountdown = _totalAnimationTime = animationTime;
    }

    private void HideAll()
    {
        _label.gameObject.SetActive(false);
        _portrait.gameObject.SetActive(false);
        _portrait2.gameObject.SetActive(false);
        _slider.gameObject.SetActive(false);
        _coinParent.SetActive(false);
        _starParent.gameObject.SetActive(false);

        _sliderLabel.text = "";
        gameObject.SetActive(true);
    }
}

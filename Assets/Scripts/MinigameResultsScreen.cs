using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameResultsScreen : MonoBehaviour
{
    [SerializeField] private Transform _listParent;
    [SerializeField] private float _delay;

    [Space()]
    [SerializeField] private List<Image> _stars;
    [SerializeField] private Color _goldStarColor;
    [SerializeField] private Color _greyStarColor;

    [Space()]
    [SerializeField] private TextMeshProUGUI _cashRewardText;

    [Space()]
    [SerializeField] private TextMeshProUGUI _happinessEffectText;
    [SerializeField] private Color _happyColor = Color.green;
    [SerializeField] private Color _unhappyColor = Color.red;
    [SerializeField] private Image _happinessCharacterPortrait;

    [Space()]
    [SerializeField] private MinigameNarrativeResults _narrative;


    public async Task ShowScore(float finalScore, RecipeData recipe, ID character, ID recipient)
    {
        foreach (Transform child in _listParent) child.gameObject.SetActive(false);

        ColorStars(finalScore);

        ShowHappiness(finalScore, character);

        _cashRewardText.text = Mathf.CeilToInt(finalScore * recipe.MoneyReward + 1).ToString();

        _narrative.Show(character, recipient, recipe, finalScore);

        gameObject.SetActive(true);
        foreach (Transform child in _listParent) {
            child.gameObject.SetActive(true);
            await Task.Delay(Mathf.RoundToInt(_delay * 1000));
        }
    }

    private void ColorStars(float finalScore)
    {
        foreach (var s in _stars) s.color = _greyStarColor;
        if (finalScore > 0.25f) _stars[0].color = _goldStarColor;
        if (finalScore > 0.50f) _stars[1].color = _goldStarColor;
        if (finalScore > 0.75f) _stars[2].color = _goldStarColor;
    }

    private void ShowHappiness(float finalScore, ID character)
    {
        if (finalScore <= 0.33f) {
            _happinessEffectText.text = "-";
            _happinessEffectText.color = _unhappyColor;
        }
        else {
            if (finalScore > 0.66f) _happinessEffectText.text = "+";
            if (finalScore > 0.90f) _happinessEffectText.text = "++";
            _happinessEffectText.color = _happyColor;
        } 
        _happinessCharacterPortrait.sprite = CharacterManager.i.GetPortrait(character);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField] private Image _bigIcon;
    [SerializeField] private TMP_Text _mame;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _steps;
    [SerializeField] private TMP_Text _difficulty;
    [SerializeField] private TMP_Text _highScore;

    [Header("Recipe Select Medals")]
    [SerializeField] private GameObject _bronzeIcon;
    [SerializeField] private GameObject _silverIcon;
    [SerializeField] private GameObject _goldIcon;
    [SerializeField] private GameObject _specialIcon;

    [SerializeField] private RecipeCharacterEffects _characterEffects;

    public void Show(RecipeData data, float highscore, ID chef, ID recipient)
    {
        _bigIcon.sprite = data.Icon;
        _mame.text = data.Name;
        _description.text = data.Description;
        _steps.text = data.ReturnSteps();

        _difficulty.text = "Difficulty: " + Utils.CapitalFirst(data.Difficulty.ToString());

        if (highscore > 0) HighscoreDisplay(highscore);
        else _highScore.text = "";

        _characterEffects.Show(chef, recipient);
    }

    public void HighscoreDisplay(float highscore)
    {
        float roundedScore = Mathf.Floor(highscore * 1000.0f);

        _highScore.text = "Highscore: " + roundedScore.ToString();

        //medal display
        _bronzeIcon.SetActive(roundedScore >= 500);
        _silverIcon.SetActive(roundedScore >= 750);
        _goldIcon.SetActive(roundedScore >= 900);
        _specialIcon.SetActive(roundedScore >= 1000);
    }
}

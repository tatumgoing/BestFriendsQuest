using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileDataDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _birthdayText;
    [SerializeField] private TextMeshProUGUI _ageText;
    [SerializeField] private TextMeshProUGUI _favoriteColorText;
    [SerializeField] private TextMeshProUGUI _pronounText;
    [SerializeField] private Image _portrait;

    public void Show(ID id)
    {
        var cMan = CharacterManager.i;
        _portrait.sprite = cMan.GetPortrait(id);
        _nameText.text = cMan.GetNameFormatted(id);
        _birthdayText.text = "Birthday: " + cMan.GetBirthdayFormatted(id);
        _ageText.text = "Age: " + cMan.GetAge(id);
        _favoriteColorText.text = "Favorite Color: " + cMan.GetFavoriteColorString(id);

        var pronoun = cMan.GetPronoun(id);
        var pronounString = Utils.CapitalFirst(pronoun.ToString().ToLower()) + "/";
        if (pronoun == Pronoun.HE) pronounString += "Him";
        if (pronoun == Pronoun.SHE) pronounString += "Her";
        if (pronoun == Pronoun.THEY) pronounString += "Them";
        _pronounText.text = pronounString;
    }
}

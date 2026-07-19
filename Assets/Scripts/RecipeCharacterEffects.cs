using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeCharacterEffects : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _character1Text;
    [SerializeField] private string _character1Template = "NAME will be happy to have cooked so well!";
    [SerializeField] private TextMeshProUGUI _character2Text;
    [SerializeField] private string _character2Template = "NAME will be happy to eat!";
    [SerializeField] private CharacterPortraitNameDisplay _character1Display;
    [SerializeField] private CharacterPortraitNameDisplay _character1DisplayLower;
    [SerializeField] private CharacterPortraitNameDisplay _character2Display;
    [SerializeField] private CharacterPortraitNameDisplay _character2DisplayLower;

    public void Show(ID character1, ID character2)
    {
        _character1Text.text = _character1Template.Replace("NAME", CharacterManager.i.GetNameFormatted(character1));
        _character2Text.text = _character2Template.Replace("NAME", CharacterManager.i.GetNameFormatted(character2));
        _character1Display.Show(character1);
        _character1DisplayLower.Show(character1);
        _character2Display.Show(character2);
        _character2DisplayLower.Show(character2);
    }
}

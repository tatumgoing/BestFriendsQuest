using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameNarrativeResults : MonoBehaviour
{
    [SerializeField] private Image _chefPortrait;
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _topText;
    [SerializeField] private string _soloTopTextTemplate = "NAME ate the DISH all by himself";
    [SerializeField] private string _giftTopTextTemplate = "NAME gave the DISH to RECIPIENT";

    [Space()]
    [SerializeField] private Image _recipientPortrait;
    [SerializeField] private TextMeshProUGUI _bottomText;
    [SerializeField] private List<string> _soloBottomTextOptions = new List<string>();
    [SerializeField] private List<string> _giftBottomTextOptions = new List<string>();

    public void Show(ID primary, ID recipient, RecipeData dish, float score)
    {
        _chefPortrait.sprite = CharacterManager.i.GetPortrait(primary);
        _itemSprite.sprite = dish.Icon;

        var gift = primary != recipient;

        _recipientPortrait.gameObject.SetActive(gift);
        _recipientPortrait.sprite = CharacterManager.i.GetPortrait(recipient);

        var bottomTextOptions = gift ? _giftBottomTextOptions : _soloBottomTextOptions;
        var stringIndex = Mathf.FloorToInt(score * (bottomTextOptions.Count - 1));
        if (score > 0.99f) _bottomText.text = bottomTextOptions[^1];
        else _bottomText.text = bottomTextOptions[stringIndex];

        var text = _bottomText.text;
        var pronoun = CharacterManager.i.GetPronoun(gift ? recipient : primary);
        _bottomText.text = text.Replace("PRONOUN", pronoun);

        var topTextString = (gift ? _giftTopTextTemplate : _soloTopTextTemplate).
            Replace("NAME", CharacterManager.i.GetNameFormatted(primary)).
            Replace("DISH", dish.Name).
            Replace("RECIPIENT", CharacterManager.i.GetNameFormatted(recipient)).
            Replace("PRONOUN", CharacterManager.i.GetPronounOwnership(primary));

        _topText.text = topTextString;
    }
}

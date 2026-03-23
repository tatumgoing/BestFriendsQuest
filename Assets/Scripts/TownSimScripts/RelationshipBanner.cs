using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RelationshipBanner : MonoBehaviour
{
    [SerializeField] private Image _portraitImg;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _relationshipLevelTxt;
    [SerializeField] private TextMeshProUGUI _relationshipNameText;
    [SerializeField] private Slider _slider;

    public void ShowRelationship(ID character, float relationshipLevel, string levelName)
    {
        _portraitImg.sprite = CharacterManager.i.GetPortrait(character);
        _nameText.text = CharacterManager.i.GetNameFormatted(character);
        _slider.value = relationshipLevel - Mathf.FloorToInt(relationshipLevel);
        _relationshipLevelTxt.text = "Level " + Mathf.FloorToInt(relationshipLevel);
        _relationshipNameText.text = levelName;
    }
}

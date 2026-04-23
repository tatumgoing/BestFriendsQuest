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

    private ID _id;

    public void ShowRelationship(ID id, float relationshipLevel, string levelName)
    {
        _id = id;
        _portraitImg.sprite = CharacterManager.i.GetPortrait(id);
        _nameText.text = CharacterManager.i.GetNameFormatted(id);
        _slider.value = relationshipLevel - Mathf.FloorToInt(relationshipLevel);
        _relationshipLevelTxt.text = "Level " + Mathf.FloorToInt(relationshipLevel);
        _relationshipNameText.text = levelName;
    }

    public void Hover() => GetComponentInParent<CharacterStatusMenu>().ShowInfoLeft(_id);
    
    public void EndHover() => GetComponentInParent<CharacterStatusMenu>().HideLeftinfo();
}

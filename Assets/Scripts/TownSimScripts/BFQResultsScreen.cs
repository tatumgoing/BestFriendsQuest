using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class BFQResultsScreen : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private QuestUIController _controller;
    [SerializeField] private CharacterPortraitNameDisplay _char1Display;
    [SerializeField] private CharacterPortraitNameDisplay _char2Display;
    [SerializeField] private GameObject _page1Parent;
    [SerializeField] private GameObject _page2Parent;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private string _titleSuccess = "Contrgratulations!";
    [SerializeField] private string _titleFail = "Better luck next time";

    [Header("Page 1")]
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private string _descriptionTemplateSucceed = "NAME1 and NAME2 brought back ITEM";
    [SerializeField] private string _descriptionTemplateFail = "NAME1 and NAME2 didn't find the ITEM";
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _bottomDescriptionText;
    [SerializeField] private string _bottomDescriptionTemplateSuccess = "Find it in the store!";
    [SerializeField] private string _bottomDescriptionTemplateFail = "Try characters that have a stronger friendship next time";

    [Header("Page 2")]
    [SerializeField] private TextMeshProUGUI _deltaRelationshipText;
    [SerializeField] private TextMeshProUGUI _page2RelationshipLevel;
    [SerializeField] private TextMeshProUGUI _page2DescriptionText;
    [SerializeField] private string _page2DescriptionTemplateSucceed = "The Quest for ITEM brought NAME1 and NAME2 closer together.";
    [SerializeField] private string _page2DescriptionTemplateFail = "The struggle for ITEM weakened NAME1 and NAME2's relationship.";
    [SerializeField] private Slider _page2Slider;

    private bool _success;

    //private float _targetSliderValue;

    public void ShowResults(RuntimeQuestData questData)
    {
        var roll = Random.Range(0, 1f);
        _success = roll < questData.SuccessChance();

        _title.text = _success ? _titleSuccess : _titleFail;
        _bottomDescriptionText.text = _success ? _bottomDescriptionTemplateSuccess : _bottomDescriptionTemplateFail;
        _deltaRelationshipText.text = "Relationship " + (_success ? " increased" : " decreased");

        _page1Parent.SetActive(true);
        _page2Parent.SetActive(false);

        _char1Display.Show(questData.Character1);
        _char2Display.Show(questData.Character2);

        _icon.sprite = questData.QuestData.unlockedItem.sprite;
        _icon.color = _success ? Color.white : Color.black;

        var name1 = CharacterManager.i.GetNameFormatted(questData.Character1);
        var name2 = CharacterManager.i.GetNameFormatted(questData.Character2);
        var item = questData.QuestData.unlockedItem.Name;

        var page1Template = _success ? _descriptionTemplateSucceed : _descriptionTemplateFail;
        var description = page1Template.Replace("NAME1", name1).Replace("NAME2", name2).Replace("ITEM", item);
        _descriptionText.text = description;

        var page2Template = _success ? _page2DescriptionTemplateSucceed : _page2DescriptionTemplateFail;
        var page2Description = page2Template.Replace("NAME1", name1).Replace("NAME2", name2).Replace("ITEM", item);
        _page2DescriptionText.text = page2Description;

        var relationshipLevel = CharacterManager.i.GetRelationship(questData.Character1, questData.Character2);
        _page2Slider.value = relationshipLevel - Mathf.Floor(relationshipLevel);
        _page2RelationshipLevel.text = "relationship level: " + Mathf.FloorToInt(relationshipLevel);

        gameObject.SetActive(true);
    }

    public void Continue()
    {
        if (_page1Parent.activeInHierarchy) {
            _page1Parent.SetActive(false);
            _page2Parent.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
            _controller.ResetQuest();
        }
    }
}

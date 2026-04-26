using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusedIslandMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _reccomendedText;
    [SerializeField] private TextMeshProUGUI _currentLevel;
    [SerializeField] private TextMeshProUGUI _completionTime;
    [SerializeField] private TextMeshProUGUI _successChance;
    [SerializeField] private QuestCharacterSelector _characterSelector;
    [SerializeField] private CharacterPortraitNameDisplay _character1Display;
    [SerializeField] private CharacterPortraitNameDisplay _character2Display;
    [SerializeField] private SelectableItem _arrow;
    [SerializeField] private SelectableItem _character2Button;
    [SerializeField] private Image _character2Backing;
    [SerializeField] private GameObject _character1QuestionMark;
    [SerializeField] private GameObject _character2QuestionMark;
    [SerializeField] private GameObject _bottomPanelParent;
    [SerializeField] private QuestMapController _controller;
     
    [SerializeField] private Color _disabledColor;
    [SerializeField] private Color _normalColor;

    private ID _id1;
    private ID _id2;
    private Quest _questData;

    public void SelectFirstCharacter() => _characterSelector.Show(SetCharacterOne);
    public void SelectSecondCharacter() => _characterSelector.Show(SetCharacterTwo, _id1, _questData.relationshipRequirement);

    public void Initialize(Quest questData)
    {
        _character1Display.GetComponent<FunAnimator>().enabled = false;
        _character2Display.GetComponent<FunAnimator>().enabled = false;

        _currentLevel.text = "";

        _bottomPanelParent.SetActive(false);

        _character1Display.Clear();
        _character2Display.Clear();

        _character1QuestionMark.SetActive(true);
        _character2QuestionMark.SetActive(true);

        _character2Backing.color = _disabledColor;
        _character2Button.SetDisabled(true);

        _arrow.SetDisabled(true);

        _questData = questData;
        _titleText.text = questData.Title;
        _reccomendedText.text = "Recommended Level: " + Mathf.Floor(questData.relationshipRequirement);

        string[] tempArray = questData.completionTime.ToString("F2").Split(char.Parse("."));
        _completionTime.text = tempArray[0] + ":" + tempArray[1] + ":00";
    }
    
    private void SetCharacterOne(ID id1)
    {
        _character1Display.Show(id1);
        _id1 = id1;

        _character2Backing.color = _normalColor;
        _character2Button.SetDisabled(false);
        _character1QuestionMark.SetActive(false);

        _character1Display.GetComponent<FunAnimator>().enabled = true;
    }

    private void SetCharacterTwo(ID id2)
    {
        _character2Display.Show(id2);
        _id2 = id2;
        _arrow.SetDisabled(false);
        _character2QuestionMark.SetActive(false);

        var level = CharacterManager.i.GetRelationship(_id1, _id2);
        _currentLevel.text = "Relationship Level: " + (Mathf.Floor(level));

        _character2Display.GetComponent<FunAnimator>().enabled = true;

        var successChance = Mathf.Clamp01(CharacterManager.i.GetRelationship(_id1, _id2) / _questData.relationshipRequirement);
        _successChance.text = Mathf.Round(successChance * 100) + "%";

        _bottomPanelParent.SetActive(true);    
    }

    public void StartQuest()
    {
        _controller.StartQuest(_id1, _id2);
        gameObject.SetActive(false);
    }
}

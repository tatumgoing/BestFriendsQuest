using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InProgressQuestMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _recommendedLevelText;
    [SerializeField] private TextMeshProUGUI _description1Text;
    [SerializeField] private string _description1Template = "CHAR1 and CHAR2 are working are to get the ITEM";
    [SerializeField] private List<string> _description2Options = new List<string>();
    [SerializeField] private TextMeshProUGUI _description2Text;
    [SerializeField] private TextMeshProUGUI _timeLeftText;
    [SerializeField] private TextMeshProUGUI _successChanceText;
    [SerializeField] private CharacterPortraitNameDisplay _char1Display;
    [SerializeField] private CharacterPortraitNameDisplay _char2Display;

    private RuntimeQuestData _questData;
    private QuestIsland _island;

    public void Initialize(RuntimeQuestData data, QuestIsland island)
    {
        _questData = data;
        _island = island;

        _titleText.text = data.QuestData.name;
        _recommendedLevelText.text = "Recommended Level: " + data.QuestData.relationshipRequirement;
        _successChanceText.text = data.GetSuccessChanceString();

        _char1Display.Show(data.Character1);
        _char2Display.Show(data.Character2);

        _description1Text.text = data.FormatTemplate(_description1Template);

        var selectedOption = Mathf.RoundToInt(data.SuccessChance() * _description2Options.Count);
        _description2Text.text = _description2Options[Mathf.Clamp(selectedOption, 0, _description2Options.Count - 1)];

        _island.TimerTextGO.SetActive(false);
    }

    private void OnDisable()
    {
        if (_island) _island.TimerTextGO.SetActive(true);
    }

    private void Update()
    {
        _timeLeftText.text = _questData.GetTimeLeftString();
    }
}

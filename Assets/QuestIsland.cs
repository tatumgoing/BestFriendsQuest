using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestIsland : MonoBehaviour
{
    [SerializeField, ReadOnly] private Quest _questData;
    [SerializeField] private Image _itemRewardImage;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _completedParent;

    private QuestMapController _controller;
    private bool _disabled;

    public Quest QuestData => _questData;
    public GameObject TimerTextGO => _timerText.gameObject;

    private void OnEnable()
    {
        if (_questData == null) return;

        if (!_controller) _controller = GetComponentInParent<QuestMapController>();
        _timerText.text = "";
        _disabled = false;   
    }

    public void Initialize(Quest data)
    {
        _questData = data;

        var completed = SaveSystem.IsBFQuestCompleted(_questData);
        _completedParent.SetActive(completed);
        GetComponent<SelectableItem>().SetDisabled(completed);
        _itemRewardImage.sprite = _questData.unlockedItem.sprite;
    }

    public void DisplayTimer(string timeLeftString)
    {
        if (timeLeftString.Contains('-')) _timerText.text = "COMPLETED!";
        else _timerText.text = timeLeftString;

        if (_timerText.gameObject.activeInHierarchy) _disabled = false;
    }

    public void Focus()
    {
        if (_controller && !_disabled) _controller.FocusIsland(GetComponent<SelectableItem>(), _questData);
    }

    public void SetDisabled(bool disabled)
    {
        _disabled = disabled; 
        GetComponent<SelectableItem>().SetDisabled(disabled);
    }
}

using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestIsland : MonoBehaviour
{
    [SerializeField, DisplayInspector] private Quest _questData;
    [SerializeField] private Image _itemRewardImage;
    [SerializeField] private TextMeshProUGUI _timerText;

    private QuestMapController _controller;
    private bool _disabled;

    public Quest QuestData => _questData;
    public GameObject TimerTextGO => _timerText.gameObject;

    private void OnEnable()
    {
        if (!_controller) _controller = GetComponentInParent<QuestMapController>();
        _timerText.text = "";
        _disabled = false;
    }

    private void Start()
    {
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InProgressQuestMenu : MonoBehaviour
{
    [SerializeField] private Image _waterSurfaceImg;
    [SerializeField] private float _waterSpeed;
    [SerializeField] private float _waterSpeedLerpFactor = 3;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _timeLeftText;
    [SerializeField] private GameObject _islandParent;
    [SerializeField] private GameObject _completeButton;
    [SerializeField] private Image _character1;
    [SerializeField] private Image _character2;
    [SerializeField] private QuestUIController _controller;

    private RuntimeQuestData _questData;
    private float _waterSpeedDelta;

    public void Show(RuntimeQuestData data)
    {
        _character1.sprite = CharacterManager.i.GetPortrait(data.Character1);
        _character2.sprite = CharacterManager.i.GetPortrait(data.Character2);

        _questData = data;
        _titleText.text = data.QuestData.name;
        _islandParent.SetActive(false);
        _timeLeftText.gameObject.SetActive(true);
        _completeButton.SetActive(false);
        _waterSpeedDelta = 0;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        _waterSurfaceImg.material.mainTextureOffset += new Vector2(_waterSpeedDelta, 0) * Time.deltaTime;

        if (!_timeLeftText.gameObject.activeInHierarchy) {
            _waterSpeedDelta = Mathf.Lerp(_waterSpeedDelta, 0, Time.deltaTime * _waterSpeedLerpFactor);
            return;
        }

        if (_questData.percentDone() >= 1) {
            _timeLeftText.gameObject.SetActive(false);
            _islandParent.SetActive(true);
            _completeButton.SetActive(true);
        }
        else {
            _waterSpeedDelta = Mathf.Lerp(_waterSpeedDelta, _waterSpeed, Time.deltaTime * _waterSpeedLerpFactor);
            _timeLeftText.text = _questData.GetTimeLeftString();
        }
    }

    public void Complete()
    {
        //gameObject.SetActive(false);
        _controller.StartWalkingAnimation();
    }
}

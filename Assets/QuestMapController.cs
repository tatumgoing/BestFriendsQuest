using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestMapController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _transitionCurve;
    [SerializeField] private Vector2 _zoomLimits;
    [SerializeField] private RectTransform _islandTargetTransform;
    [SerializeField] private float _transitionTime;
    [SerializeField] private FocusedIslandMenu _focusedIslandMenu;
    [SerializeField] private InProgressQuestMenu _inProgressIslandMenu;
    [SerializeField] private GameObject _mapButton;
    [SerializeField] private QuestUIController _controller;
    [SerializeField] private RectTransform _boat;
    [SerializeField] private RectTransform _boatStartPos;

    private Vector2 _originalPosition;
    private float _originalScale;
    private float _targetScale;
    private float _transitionTimeLeft;
    private Vector2 _targetPosition;
    private SelectableItem _currentlyFocusedIsland;
    private RectTransform _rTransform;
    private Quest _questData;
    private List<QuestIsland> _islands;
    private RuntimeQuestData _currentQuestData;

    private void Awake()
    {
        _rTransform = GetComponent<RectTransform>();
        _islands = GetComponentsInChildren<QuestIsland>(true).ToList();
    }

    private void OnEnable()
    {
        _currentQuestData = null;
        _focusedIslandMenu.gameObject.SetActive(false);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        if (_islands.Count > 0) foreach (var i in _islands) i.SetDisabled(false);
    }

    private void Update()
    {
        if (_transitionTimeLeft <= 0) return;

        _transitionTimeLeft -= Time.deltaTime;
        var progress = _transitionTimeLeft/ _transitionTime;
        progress = _transitionCurve.Evaluate(1 - progress);

        transform.localScale = Mathf.Lerp(_originalScale, _targetScale, progress) * Vector3.one;
        _rTransform.anchoredPosition = Vector2.Lerp(_originalPosition, _targetPosition, progress);

        if (_transitionTimeLeft <= 0) FinishTransition();
    }

    public void UpdateIslands(RuntimeQuestData questData)
    {
        var selected = _islands.Where(x => x.QuestData == questData.QuestData).FirstOrDefault();
        if (selected == default) {
            _currentQuestData = null;
            return;
        }

        _currentQuestData = questData;
        selected.DisplayTimer(questData.GetTimeLeftString());
        foreach (var i in _islands) if (i != selected) i.SetDisabled(true);

        var boatPercent = Mathf.Clamp01(questData.percentDone()) * 0.7f;
        _boat.anchoredPosition = Vector2.Lerp(_boatStartPos.anchoredPosition, selected.GetComponent<RectTransform>().anchoredPosition, boatPercent);

    }

    private void FinishTransition()
    {
        if (transform.localScale.x > 1) {
            if (_currentQuestData == null) _focusedIslandMenu.gameObject.SetActive(true);
            else _inProgressIslandMenu.gameObject.SetActive(true);
        }
        transform.localScale = _targetScale * Vector3.one;
        _rTransform.anchoredPosition = _targetPosition;
    }

    public void FocusIsland(SelectableItem islandButton, Quest questData)
    {
        _questData = questData;
        _currentlyFocusedIsland = islandButton;

        if (_currentQuestData != null && questData == _currentQuestData.QuestData) _inProgressIslandMenu.Initialize(_currentQuestData, _currentlyFocusedIsland.GetComponent<QuestIsland>());
        else _focusedIslandMenu.Initialize(questData);

        ZoomIn(islandButton.GetComponent<RectTransform>());
    }

    private void ZoomIn(RectTransform islandParent)
    {
        _currentlyFocusedIsland.SetDisabled(true);
        _mapButton.SetActive(false);
        var targetPosition = _islandTargetTransform.anchoredPosition - (islandParent.anchoredPosition * _zoomLimits.y);
        Transition(_zoomLimits.y, targetPosition);
    }

    [ButtonMethod]
    public void ZoomOut()
    {
        if (_currentlyFocusedIsland) {
            _currentlyFocusedIsland.SetDisabled(false);
            _currentlyFocusedIsland = null;
        }

        _inProgressIslandMenu.gameObject.SetActive(false);
        _focusedIslandMenu.gameObject.SetActive(false);

        _mapButton.SetActive(true);
        Transition(_zoomLimits.x, Vector2.zero);
    }    

    private void Transition(float targetScale, Vector2 targetPosition)
    {
        _originalScale = transform.localScale.x;
        _targetScale = targetScale;

        _originalPosition = _rTransform.anchoredPosition;
        _targetPosition = targetPosition;

        _transitionTimeLeft = _transitionTime;
    }

    public void StartQuest(ID id1, ID id2)
    {
        _controller.StartQuest(_questData, id1, id2);
        ZoomOut();
    }
}

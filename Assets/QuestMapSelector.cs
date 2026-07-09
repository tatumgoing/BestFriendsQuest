using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QuestMapSelector : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<MapData> _mapOptions = new List<MapData>();
    [SerializeField] private RectTransform _optionsParent;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _moveDist = 800;
    [SerializeField] private float _startPos = -300;
    [SerializeField] private SelectableItem _prevButton;
    [SerializeField] private SelectableItem _nextButton;
    [SerializeField] private SelectableItem _selectButton;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _mapListParent;
    [SerializeField] private QuestUIController _controller;

    private List<MapOptionBFQuest> _spawnedMaps = new List<MapOptionBFQuest>();
    private int _currentMapIndex;
    private bool  _animating = false;

    private void OnEnable()
    {
        if (_mapOptions.Count == 0) Initialize();

        BuildList();
    }

    private void Start()
    {
        UpdateMapPos(_startPos);
        _prevButton.SetDisabled(true);
    }

    private void Initialize()
    {
        _mapOptions = Resources.LoadAll<MapData>("MapBundles").OrderBy(x => x.NumRequiredToUnlock).ToList();
        _currentMapIndex = 0;
    }

    private void BuildList()
    {
        foreach (var m in _spawnedMaps) Destroy(m.gameObject);
        _spawnedMaps.Clear();

        foreach (var m in _mapOptions) {
            var newMap = Instantiate(_prefab, _mapListParent).GetComponent<MapOptionBFQuest>();
            newMap.Initiailize(m, SaveSystem.NumQuestsCompleted() >= m.NumRequiredToUnlock);
            _spawnedMaps.Add(newMap);
        }
    }

    public void Next() {
        _currentMapIndex += 1;
        Transition(-_moveDist);
    }

    public void Previous() {
        _currentMapIndex -= 1;

        Transition(_moveDist);
    }

    public void SelectMap()
    {
        var selected = _spawnedMaps[_currentMapIndex].Data;
        SaveSystem.SaveRegion(selected.Name);
        _controller.SelectMap(selected);
        gameObject.SetActive(false);
    }

    private void UpdateMapPos(float x)
    {
        var pos = _optionsParent.anchoredPosition;
        pos.x = x;
        _optionsParent.anchoredPosition = pos;
    }
    
    private async void Transition(float move)
    {
        while (_animating) {
            await Task.Delay(10);
        }
        _animating = true;

        float timeLeft = _transitionTime;
        var startX = _optionsParent.anchoredPosition.x;
        var targetX = startX + move;

        _prevButton.SetDisabled(_currentMapIndex == 0);
        _nextButton.SetDisabled(-_currentMapIndex == _spawnedMaps.Count-1);

        _selectButton.SetDisabled(!_spawnedMaps[_currentMapIndex].Unlocked);

        while (timeLeft > 0) {

            var progress = _curve.Evaluate(1 - timeLeft / _transitionTime);
            UpdateMapPos(Mathf.Lerp(startX, targetX, progress));

            int deltaMs = Mathf.RoundToInt(Time.deltaTime * 1000);
            timeLeft -= deltaMs / 1000f;
            await Task.Delay(deltaMs);
        }

        UpdateMapPos(targetX);

        _animating = false;
    }
}

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
    [SerializeField] private float _moveDist = 1.5f;
    [SerializeField] private SelectableItem _prevButton;
    [SerializeField] private SelectableItem _nextButton;

    private bool  _animating = false;

    public void Next() => Transition(_moveDist);
    public void Previous() => Transition(-_moveDist);

    private void Initialize()
    {
        _mapOptions = Resources.LoadAll<MapData>("MapBundles").ToList();
    }

    private void OnEnable()
    {
        if (_mapOptions.Count == 0) Initialize();

        var maps = GetComponentsInChildren<MapOptionBFQuest>(true);
        for (int i = 0; i < maps.Length; i++)
        {
            if (i > _mapOptions.Count) maps[i].Initiailize(_mapOptions[i]);
            else maps[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        UpdateMapPos(1600);
        _prevButton.SetDisabled(true);
    }

    public void SelectMap()
    {
        SaveSystem.SaveRegion("testRegion");
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


        _prevButton.SetDisabled(targetX >= 1600);
        _nextButton.SetDisabled(targetX <= -1600);

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

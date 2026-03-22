using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StirMinigame : Subgame
{
    [SerializeField] private CircleMovement _target;
    [SerializeField] private CanvasGroup _targetCanvasGroup;
    [SerializeField] private float _minOpacity = 0.5f;
    [SerializeField] private float _distanceThreshold = 30;

    [Header("Scoring")]
    [SerializeField] private float _rewardScore;
    [SerializeField] private float _failScorePenalty;

    [Header("Audio")]
    [SerializeField] private Sound _stirringSFX;

    private float _changeTimer;
    private bool _clockwise;
    private float _successTime;

    private void Update()
    {
        _changeTimer -= Time.deltaTime;
        if (_changeTimer < 0) ChangeSpeed();

        var dist = Vector2.Distance(Input.mousePosition, _target.Position);
        if (dist < _distanceThreshold) _successTime += Time.deltaTime;
        controller.UpdateSlider(_successTime / data.TargetTime);
        if (_successTime >= data.TargetTime) {
            gameObject.SetActive(false);
            controller.CompleteSubgame();
        }

        _targetCanvasGroup.alpha = Mathf.Lerp(_targetCanvasGroup.alpha, dist < _distanceThreshold ? 1 : _minOpacity, 5 * Time.deltaTime);
    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        _successTime = 0;
        _stirringSFX.Play();
        ChangeSpeed();
    }

    protected override void Initialize()
    {
        base.Initialize();

        _stirringSFX = Instantiate(_stirringSFX);
    }

    private void ChangeSpeed()
    {
        if (data == null) return;

        _clockwise = !_clockwise;
        var speed = Mathf.Lerp(data.MinStirSpeed, data.MaxStirSpeed, 1 - controller.TimeLeftPercent);
        _target.SetMoveSpeed(speed * (_clockwise ? 1 : -1));

        _changeTimer = Random.Range(data.ChangeSpeedFrequency.x, data.ChangeSpeedFrequency.y);
    }
}

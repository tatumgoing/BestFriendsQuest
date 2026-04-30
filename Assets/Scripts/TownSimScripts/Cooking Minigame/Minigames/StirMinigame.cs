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

    protected override void Update()
    {
        _changeTimer -= Time.deltaTime;
        if (_changeTimer < 0) ChangeSpeed();

        var dist = Vector2.Distance(Input.mousePosition, _target.Position);
        _targetCanvasGroup.alpha = Mathf.Lerp(_targetCanvasGroup.alpha, dist < _distanceThreshold ? 1 : _minOpacity, 5 * Time.deltaTime);
        if (dist < _distanceThreshold)
        {
            _stirringSFX.SetPercentVolume(100, 10 * Time.deltaTime);
            SuccessTime += Time.deltaTime;
        }
        else
        {
            _stirringSFX.SetPercentVolume(0, 10 * Time.deltaTime);
        }

            base.Update();
    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        ShowCam(0);
        _stirringSFX.PlaySilent();
        ChangeSpeed();
    }

    protected override void Initialize()
    {
        base.Initialize();

        _stirringSFX = Instantiate(_stirringSFX);
    }

    private void OnDisable()
    {
        _stirringSFX.Stop();
        ResetCam();
    }

    private void ChangeSpeed()
    {
        if (Data == null) return;

        _clockwise = !_clockwise;
        var speed = Mathf.Lerp(Data.MinStirSpeed, Data.MaxStirSpeed, 1 - Controller.TimeLeftPercent);
        _target.SetMoveSpeed(speed * (_clockwise ? 1 : -1));

        _changeTimer = Random.Range(Data.ChangeSpeedFrequency.x, Data.ChangeSpeedFrequency.y);
    }
}

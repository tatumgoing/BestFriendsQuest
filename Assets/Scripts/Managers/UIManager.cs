using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private Fade _fade;
    [SerializeField] private GameObject _cameraFlash; 

    public UnityEvent OnTabSwitch = new UnityEvent();

    public Fade Fade => _fade;
    public void SwitchTab() => OnTabSwitch.Invoke();
    private void Awake() => i = this;

    private string GetTimeString(int seconds)
    {
        seconds = Mathf.FloorToInt(seconds);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        return timeString;
    }

    public void FlashCamera()
    {
        _cameraFlash.SetActive(false);
        _cameraFlash.SetActive(true);
    }
}

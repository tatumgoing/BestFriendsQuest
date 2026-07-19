using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Clock : MonoBehaviour
{
    public TMP_Text timeDisplay;
    [SerializeField] private GameObject _am;
    [SerializeField] private GameObject _pm;
    [SerializeField] private TextMeshProUGUI _date;
    [SerializeField] private TextMeshProUGUI _season;

    void Update()
    {
        timeDisplay.text = GetTime();

        bool isPM = DateTime.Now.Hour >= 12;
        if (_am) _am.SetActive(!isPM);
        if (_pm) _pm.SetActive(isPM);

        if (_date) {
            _date.text = DateTime.Now.Date.ToString("dd-MM");
        }
        if (_season) {
            var month = DateTime.Now.Month;
            if (month < 3) _season.text = "winter";
            else if (month < 6) _season.text = "spring";
            else if (month < 9) _season.text = "summer";
            else if (month < 12) _season.text = "fall";
            else _season.text = "winter";
        }
    }

    private string GetTime()
    {
        return DateTime.Now.ToString("hh:mm");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BFQInProgressScreen : MonoBehaviour
{
    public BFQManager questManager;

    [Header("Timer")]
    public DateTime startTime;
    public DateTime currentTime;
    public DateTime endTime;

    public TMP_Text timerText;


    void Start()
    {
        questManager = BFQManager.i;
    }

    void Update()
    {
        currentTime = DateTime.Now;

        if (endTime != null &&  DateTime.Compare(currentTime, endTime) < 0) {
            timerText.text = (endTime - currentTime).ToString(@"hh\:mm\:ss");
        }
    }
    public void SetTime(Quest associatedQuest)
    {
        startTime = DateTime.Now;
        endTime = DateTime.Now.AddHours(associatedQuest.completionTime);

        Debug.Log(startTime + " and end at " + endTime);
    }

}
